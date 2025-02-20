﻿using DS4Windows.Shared.Devices.HID;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DS4Windows.Shared.Devices.Util
{
    /// <summary>
    ///     Utility class to add device arrival/removal notifications to WPF window.
    /// </summary>
    /// <remarks>Original source: https://gist.github.com/emoacht/73eff195317e387f4cda</remarks>
    public class DeviceNotificationListener : IDeviceNotificationListener
    {
        private readonly ILogger<DeviceNotificationListener> logger;

        public event Action<string> DeviceArrived;

        public event Action<string> DeviceRemoved;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DEVICECHANGE)
            {
                DEV_BROADCAST_HDR hdr;

                switch ((int)wParam)
                {
                    case DBT_DEVICEARRIVAL:
                        logger.LogDebug("Device added.");

                        hdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_HDR));

                        if (hdr.dbch_devicetype == DBT_DEVTYP_DEVICEINTERFACE)
                        {
                            var deviceInterface =
                                (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(lParam,
                                    typeof(DEV_BROADCAST_DEVICEINTERFACE));

                            DeviceArrived?.Invoke(deviceInterface.dbcc_name);
                        }

                        break;

                    case DBT_DEVICEREMOVECOMPLETE:
                        logger.LogDebug("Device removed.");

                        hdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_HDR));

                        if (hdr.dbch_devicetype == DBT_DEVTYP_DEVICEINTERFACE)
                        {
                            var deviceInterface =
                                (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(lParam,
                                    typeof(DEV_BROADCAST_DEVICEINTERFACE));

                            DeviceRemoved?.Invoke(deviceInterface.dbcc_name);
                        }

                        break;
                }
            }

            return IntPtr.Zero;
        }

        #region Win32

        [DllImport(nameof(PInvoke.User32), SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(
            IntPtr hRecipient,
            IntPtr NotificationFilter,
            uint Flags);

        [DllImport(nameof(PInvoke.User32), SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterDeviceNotification(IntPtr Handle);

        private const uint DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;

        private const int WM_DEVICECHANGE = 0x0219;

        private const int
            DBT_DEVICEARRIVAL =
                0x8000; // Device event when a device or piece of media has been inserted and becomes available

        private const int
            DBT_DEVICEREMOVECOMPLETE =
                0x8004; // Device event when a device or piece of media has been physically removed

        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_HDR
        {
            public readonly uint dbch_size;
            public readonly uint dbch_devicetype;
            public readonly uint dbch_reserved;
        }

        private const uint DBT_DEVTYP_VOLUME = 0x00000002;
        private const uint DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_VOLUME
        {
            public readonly uint dbcv_size;
            public readonly uint dbcv_devicetype;
            public readonly uint dbcv_reserved;
            public readonly uint dbcv_unitmask;
            public readonly ushort dbcv_flags;
        }

        private const ushort DBTF_MEDIA = 0x0001; // Media in drive changed.
        private const ushort DBTF_NET = 0x0002; // Network drive is changed.

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public uint dbcc_size;
            public uint dbcc_devicetype;
            public readonly uint dbcc_reserved;
            public Guid dbcc_classguid;

            // To get value from lParam of WM_DEVICECHANGE, this length must be longer than 1.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public readonly string dbcc_name;
        }

        // Identifier: GUID_DEVINTERFACE_USB_DEVICE
        // Class GUID: {A5DCBF10-6530-11D2-901F-00C04FB951ED}
        private static readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new("A5DCBF10-6530-11D2-901F-00C04FB951ED");

        #endregion

        #region Start/End

        private HwndSource _handleSource;
        private IntPtr _notificationHandle;

        public DeviceNotificationListener(ILogger<DeviceNotificationListener> logger)
        {
            this.logger = logger;
        }

        public void StartListen(Window window, Guid interfaceGuid)
        {
            _handleSource = PresentationSource.FromVisual(window) as HwndSource;
            if (_handleSource != null)
            {
                RegisterUsbDeviceNotification(_handleSource.Handle, interfaceGuid);
                _handleSource.AddHook(WndProc);
            }
            else
            {
                logger.LogError("Failed to register device listener window message hook");
            }
        }

        public void EndListen()
        {
            if (_handleSource != null)
            {
                UnregisterUsbDeviceNotification();
                _handleSource.RemoveHook(WndProc);
            }
            else
            {
                logger.LogError("Failed to unregister device listener window message hook");
            }
        }

        private void RegisterUsbDeviceNotification(IntPtr windowHandle, Guid interfaceGuid)
        {
            var dbcc = new DEV_BROADCAST_DEVICEINTERFACE
            {
                dbcc_size = (uint)Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE)),
                dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE,
                dbcc_classguid = interfaceGuid
            };

            var notificationFilter = Marshal.AllocHGlobal(Marshal.SizeOf(dbcc));
            Marshal.StructureToPtr(dbcc, notificationFilter, true);

            _notificationHandle =
                RegisterDeviceNotification(windowHandle, notificationFilter, DEVICE_NOTIFY_WINDOW_HANDLE);
            if (_notificationHandle == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to register device notifications.");
        }

        private void UnregisterUsbDeviceNotification()
        {
            if (_notificationHandle != IntPtr.Zero)
                UnregisterDeviceNotification(_notificationHandle);
        }

        #endregion
    }
}
