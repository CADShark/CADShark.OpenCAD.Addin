using CADShark.Common.Logging;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Input;

namespace CADShark.OpenCAD.Addin
{
    internal class HandlingProgressBar
    {
        private readonly SldWorks _swApp;
        private readonly UserProgressBar _swPrgBar;
        private bool _status;
        private bool _cancelRequested;
        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(HandlingProgressBar));

        public int GetProgressBarUpperBound;
        public string Title;

        public int Pos;

        public HandlingProgressBar()
        {
            
        }
        public HandlingProgressBar(SldWorks swApp)
        {
            _swApp = swApp;
            _status = _swApp.GetUserProgressBar(out _swPrgBar);
        }
        internal bool StartProgressBar()
        {
            return _swPrgBar.Start(0, GetProgressBarUpperBound, Title);
        }
        internal bool UpdateTitle()
        {
            return _swPrgBar.UpdateTitle(Title);
        }

        internal bool UserCancel()
        {
            if (!_cancelRequested && (int)swUpdateProgressError_e.swUpdateProgressError_UserCancel ==
                _swPrgBar.UpdateProgress(Pos))
            {
                _cancelRequested = _swApp.SendMsgToUser2("Cancel operation?", (int)swMessageBoxIcon_e.swMbWarning,
                    (int)swMessageBoxBtn_e.swMbYesNo) == (int)swMessageBoxResult_e.swMbHitYes;

                if (_cancelRequested)
                {
                    _swPrgBar.End();
                    return true; // Return true to indicate cancellation
                }

                return false; // Return false to indicate operation should continue
            }

            return false; // Return false to indicate operation should continue
        }
        internal void ProcessKeyboardInput()
        {
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                _cancelRequested = true;
                Logger.Info("Cancellation requested by user.");
            }
        }
        internal int UpdateProgress()
        {
            return _swPrgBar.UpdateProgress(Pos);
        }
        internal bool EndProgress()
        {
            return _swPrgBar.End();
        }
        
    }
}
