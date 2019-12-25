﻿using System.IO;

namespace SplitScreenMe.Core {
    /// <summary>
    /// 
    /// </summary>
    public struct SearchStorageInfo {
        public DriveInfo Drive { get; private set; }
        public string Info { get; private set; }

        public SearchStorageInfo(DriveInfo drive) {
            Drive = drive;
            Info = "";
        }

        public void SetInfo(string info) {
            Info = info;
        }

        public override string ToString() {
            return Info;
        }
    }
}