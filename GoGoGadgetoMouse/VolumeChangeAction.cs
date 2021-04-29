using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace GoGoGadgetoMouse
{
    class VolumeChangeAction {

        private readonly CoreAudioController mController;

        public VolumeChangeAction()
        {
            mController = new AudioSwitcher.AudioApi.CoreAudio.CoreAudioController();
        }

        public void Update(MouseInterceptor.MouseEventArgs e) {
            if (e.Delta >= 120) {
                VolumeUp();
            } else if (e.Delta <= -120) {
                VolumeDown();
            }
        }

        private void Mute() {
            mController.DefaultPlaybackDevice.Mute(!mController.DefaultPlaybackDevice.IsMuted);
        }

        private void VolumeDown() {
            mController.DefaultPlaybackDevice.Volume -= 4;
        }

        private void VolumeUp() {
            mController.DefaultPlaybackDevice.Volume += 4;
        }
    }
}
