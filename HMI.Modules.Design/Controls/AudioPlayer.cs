// SPDX-License-Identifier: GPL-3.0-only
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Common.Presentation.Views;

namespace HMI.Modules.Design.Controls
{
    [ExcludeFromCodeCoverage]
    public class AudioPlayer : Control
    {
        MediaElement mediaElement;
        Image uxpause;
        Image uxplay;
        Image uxstop;

        public static readonly DependencyProperty MediaUriProperty = 
            DependencyProperty.Register("MediaUri", typeof(Uri), typeof(AudioPlayer), null);
        public Uri MediaUri
        {
            get
            {
                return (Uri)base.GetValue(MediaUriProperty);
            }
            set
            {
                base.SetValue(MediaUriProperty, value);
            }
        }
        static AudioPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AudioPlayer), new FrameworkPropertyMetadata(typeof(AudioPlayer)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.mediaElement = this.FindTemplateChild<MediaElement>("uxMediaElement");
            this.mediaElement.MediaEnded += new RoutedEventHandler(this.mediaElement_MediaEnded);

            this.uxplay = this.FindTemplateChild<Image>("uxPlay");
            this.uxplay.MouseLeftButtonDown += new MouseButtonEventHandler(this.play_MouseLeftButtonDown);

            this.uxpause = this.FindTemplateChild<Image>("uxPause");
            this.uxpause.MouseLeftButtonDown += new MouseButtonEventHandler(this.pause_MouseLeftButtonDown);

            this.uxstop = this.FindTemplateChild<Image>("uxStop");
            this.uxstop.MouseLeftButtonDown += new MouseButtonEventHandler(this.stop_MouseLeftButtonDown);
        }
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            stop();
        }
        private void stop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stop();
        }
        private void pause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pause();
        }
        private void play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            play();
        }

        void stop()
        {
            mediaElement.Stop();
            showplay();
        }
        void showplay()
        {
            uxplay.Visibility = Visibility.Visible;
            uxpause.Visibility = Visibility.Hidden;
            uxstop.Visibility = Visibility.Hidden;
        }
        void pause()
        {
            mediaElement.Pause();
            showplay();
        }
        void play()
        {
            mediaElement.Play();
            showstop();
        }
        void showstop()
        {
            uxplay.Visibility = Visibility.Hidden;
            uxpause.Visibility = Visibility.Visible;
            uxstop.Visibility = Visibility.Visible;
        }
    }
}
