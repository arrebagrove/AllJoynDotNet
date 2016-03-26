﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;

namespace AllJoynDotNet
{
    public partial class BusAttachment
    {
        private static Dictionary<IntPtr, WeakReference<BusAttachment>> _sBusAttachmentMap = new Dictionary<IntPtr, WeakReference<BusAttachment>>();
        internal static BusAttachment GetBusAttachment(IntPtr key)
        {
            BusAttachment bus = null;
            if (_sBusAttachmentMap.ContainsKey(key))
            {
                var instance = _sBusAttachmentMap[key];
                instance.TryGetTarget(out bus);
            }
            return bus;
        }

        private readonly string _busName;
        private readonly bool _allowRemoteMessages;

        static BusAttachment()
        {
            Init.Initialize();
        }

        #region Construct and destroy

        private static IntPtr CreateHandle(string busName, bool allowRemoteMessages)
        {
            var handle = alljoyn_busattachment_create(busName, allowRemoteMessages.ToQccBool());
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Could not create bus attachment");
            return handle;
        }

        public BusAttachment() : this(GenerateBusName(), true)
        {
        }

        public BusAttachment(string busName, bool allowRemoteMessages) : base(CreateHandle(busName, allowRemoteMessages))
        {
            _busName = busName;
            _allowRemoteMessages = allowRemoteMessages;
            _sBusAttachmentMap.Add(Handle, new WeakReference<BusAttachment>(this));
        }

        protected override void Dispose(bool disposing)
        {
            if (IsStarted && !IsStopping)
                Stop();
            if (IsStopping)
                Join();
            if (Handle != IntPtr.Zero)
                alljoyn_busattachment_destroy(Handle);
            base.Dispose(disposing);
        }

        #endregion

        public string UniqueName
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    throw new InvalidOperationException("Bus is not connected");
                var p = alljoyn_busattachment_getuniquename(Handle);
                return Marshal.PtrToStringAnsi(p);
            }
        }

        #region Bus Attachment Lifecycle

        public void Start()
        {
            var result = alljoyn_busattachment_start(Handle);
            if (result != 0)
                throw new AllJoynException(result, "Failed to start bus attachment.");
        }

        public void Stop()
        {
            var result = alljoyn_busattachment_stop(Handle);
            if (result != 0)
                throw new AllJoynException(result, "Failed to stop bus attachment.");
        }

        public void Join()
        {
            var result = alljoyn_busattachment_join(Handle);
            if (result != 0)
                throw new AllJoynException(result); //, "Failed to join bus attachment.");
        }

        public void Connect(string connectSpec = null)
        {
            var result = alljoyn_busattachment_connect(Handle, connectSpec);
            if (result != 0)
                throw new AllJoynException(result); //, "Failed to connect bus attachment.");
            Debug.WriteLine($"BusAttachment connect succeeded. Bus name = {_busName}");
        }
        
        public void Disconnect()
        {
            var result = alljoyn_busattachment_disconnect(Handle, "");
            if (result != 0)
                throw new AllJoynException(result, "Failed to disconnect bus attachment");
        }

        public bool IsStarted
        {
            get
            {
                return (alljoyn_busattachment_isstarted(Handle) == 1);
            }
        }

        public bool IsStopping
        {
            get
            {
                return (alljoyn_busattachment_isstopping(Handle) == 1);
            }
        }

        public bool IsConnected
        {
            get
            {
                return (alljoyn_busattachment_isconnected(Handle) == 1);
            }
        }

        #endregion

        #region Interfaces

        public void CreateInterfacesFromXml(string xml)
        {
            var result = alljoyn_busattachment_createinterfacesfromxml(Handle, xml);
            if (result != 0)
                throw new AllJoynException(result, "Failed to create interface");
        }

        public InterfaceDescription GetInterface(string name)
        {
            var handle = alljoyn_busattachment_getinterface(Handle, name);
            return InterfaceDescription.Create(handle);
        }

        public InterfaceDescription[] GetInterfaces()
        {
            ulong numIfaces = (ulong)alljoyn_busattachment_getinterfaces(Handle, IntPtr.Zero,  UIntPtr.Zero);
            IntPtr[] ifaces = new IntPtr[(int)numIfaces];
            GCHandle gch = GCHandle.Alloc(ifaces, GCHandleType.Pinned);
            ulong numIfacesFilled = (ulong)alljoyn_busattachment_getinterfaces(Handle,
                gch.AddrOfPinnedObject(), (UIntPtr)numIfaces);
            gch.Free();
            if (numIfaces != numIfacesFilled)
            {
                // Warn? 
            }
            InterfaceDescription[] ret = new InterfaceDescription[(int)numIfacesFilled];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new InterfaceDescription(ifaces[i]);
            }
            return ret;
        }

        public void WhoImplementsInterfaces(string[] interfaces)
        {
            var result = alljoyn_busattachment_whoimplements_interfaces(Handle, interfaces, (UIntPtr)interfaces.Length);
            if (result > 0)
                throw new AllJoynException(result);
        }

        #endregion

        public void RegisterAboutListener(AboutListener listener)
        {
            alljoyn_busattachment_registeraboutlistener(Handle, listener.Handle);
        }
        

        private static string GenerateBusName()
        {
#if NETFX_CORE
            string name  = Windows.ApplicationModel.Package.Current.Id.FamilyName;
#else
            //TODO: Xamarin

            var assy = Assembly.GetEntryAssembly();
            if (assy == null)
                assy = Assembly.GetCallingAssembly();

            string name = assy.GetName().Name;
#endif
            return System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z0-9-.]+", "");
        }
    }
}