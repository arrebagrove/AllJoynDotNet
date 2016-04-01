// Generated from SessionPortListener.h

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace AllJoynDotNet
{
		/// <summary>
		/// Type for the AcceptSessionJoiner callback.
		/// </summary>
		/// <remarks>
		/// <para>Accept or reject an incoming JoinSession request. The session does not exist until
		/// after this function returns.
		/// </para>
		/// <para>This callback is only used by session creators. Therefore it is only called on listeners
		/// passed to alljoyn_busattachment_bindsessionport.
		/// </para>
		/// </remarks>
		/// <param name="context">the context pointer that was passed to alljoyn_sessionportlistener_create</param>
		/// <param name="sessionPort">Session port that was joined.</param>
		/// <param name="joiner">Unique name of potential joiner.</param>
		/// <param name="opts">Session options requested by the joiner.</param>
		/// <returns>Return true if JoinSession request is accepted. false if rejected.</returns>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate Int32 alljoyn_sessionportlistener_acceptsessionjoiner_ptr(IntPtr context, UInt16 sessionPort, [MarshalAs(UnmanagedType.LPStr)]string joiner, IntPtr opts);
		// typedef QCC_BOOL (AJ_CALL * alljoyn_sessionportlistener_acceptsessionjoiner_ptr)(const void* context, alljoyn_sessionport sessionPort,
		// const char* joiner,  const alljoyn_sessionopts opts);
		// 

		/// <summary>
		/// Type for the SessionJoined callback.
		/// </summary>
		/// <remarks>
		/// <para>Called by the bus when a session has been successfully joined. The session is now fully up.
		/// </para>
		/// <para>This callback is only used by session creators. Therefore it is only called on the alljoyn_sessionportlistener_callbacks
		/// passed in when calling alljoyn_busattachment_bindsessionport.
		/// </para>
		/// </remarks>
		/// <param name="context">the context pointer that was passed to alljoyn_sessionportlistener_create</param>
		/// <param name="sessionPort">Session port that was joined.</param>
		/// <param name="id">Id of session.</param>
		/// <param name="joiner">Unique name of the joiner.</param>
		/// 
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void alljoyn_sessionportlistener_sessionjoined_ptr(IntPtr context, UInt16 sessionPort, IntPtr id, [MarshalAs(UnmanagedType.LPStr)]string joiner);
		// typedef void (AJ_CALL * alljoyn_sessionportlistener_sessionjoined_ptr)(const void* context, alljoyn_sessionport sessionPort,
		// alljoyn_sessionid id, const char* joiner);
		// 

		/// <summary>
		/// Structure used during alljoyn_sessionportlistener_create to provide callbacks into C.
		/// </summary>
		/// 
		[StructLayout(LayoutKind.Sequential)]
		internal partial class alljoyn_sessionportlistener_callbacks
		{
			public alljoyn_sessionportlistener_acceptsessionjoiner_ptr accept_session_joiner;
			public alljoyn_sessionportlistener_sessionjoined_ptr session_joined;
			//
			///**
			//* Accept or reject an incoming JoinSession request. The session does not
			//* exist until this after this function returns.
			//*/
			//alljoyn_sessionportlistener_acceptsessionjoiner_ptr accept_session_joiner;
			///**
			//* Called by the bus when a session has been successfully joined. The session
			//* is now fully up.
			//*/
			//alljoyn_sessionportlistener_sessionjoined_ptr session_joined;
			//
		}
		// typedef struct {
		// /**
		// * Accept or reject an incoming JoinSession request. The session does not
		// * exist until this after this function returns.
		// */
		// alljoyn_sessionportlistener_acceptsessionjoiner_ptr accept_session_joiner;
		// /**
		// * Called by the bus when a session has been successfully joined. The session
		// * is now fully up.
		// */
		// alljoyn_sessionportlistener_sessionjoined_ptr session_joined;
		// } alljoyn_sessionportlistener_callbacks;
		// 

//
    public partial class SessionPortListener : AllJoynWrapper
    {
        internal SessionPortListener(IntPtr handle) : base(handle) { }
		/// <summary>
		/// Create an alljoyn_sessionportlistener which will trigger the provided callbacks, passing along the provided context.
		/// </summary>
		/// <param name="callbacks">Callbacks to trigger for associated events.</param>
		/// <param name="context">Context to pass to callback functions</param>
		/// <returns>Handle to newly allocated alljoyn_sessionportlistener.</returns>
		[DllImport(Constants.DLL_IMPORT_TARGET)]
		internal static extern IntPtr alljoyn_sessionportlistener_create(alljoyn_sessionportlistener_callbacks callbacks, IntPtr context);
		// extern AJ_API alljoyn_sessionportlistener AJ_CALL alljoyn_sessionportlistener_create(const alljoyn_sessionportlistener_callbacks* callbacks,const void* context);

		/// <summary>
		/// Destroy an alljoyn_sessionportlistener.
		/// </summary>
		/// <param name="listener">alljoyn_sessionportlistener to destroy.</param>
		/// 
		[DllImport(Constants.DLL_IMPORT_TARGET)]
		internal static extern void alljoyn_sessionportlistener_destroy(IntPtr listener);
		// extern AJ_API void AJ_CALL alljoyn_sessionportlistener_destroy(alljoyn_sessionportlistener listener);


    }
}