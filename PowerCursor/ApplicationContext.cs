﻿using GoGoGadgetoMouse.Properties;
using System;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    public class ApplicationContext : System.Windows.Forms.ApplicationContext {
        private readonly NotifyIcon mTrayIcon;
        private readonly MouseService mMouseService;

        public ApplicationContext() {
            mMouseService = MouseService.The();

            mTrayIcon = new NotifyIcon() {
                Icon = Resources.MouseIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Settings", Settings),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };
        }

        void Settings(object sender, EventArgs e) {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        void Exit(object sender, EventArgs e) {
            mTrayIcon.Visible = false;
            Application.Exit();
        }
    }

}
