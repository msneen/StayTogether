using System;
using Android.Views;

namespace StayTogether.Droid.Swipe
{
    public class SwipeGestureListener : GestureDetector.SimpleOnGestureListener
    {
        public event EventHandler OnSwipeDown;
        public event EventHandler OnSwipeUp;
        public event EventHandler OnSwipeLeft;
        public event EventHandler OnSwipeRight;
        public event OnUpEventHandler OnUp;

        private static readonly int SWIPE_THRESHOLD = 100;
        private static readonly int SWIPE_VELOCITY_THRESHOLD = 100;

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            var onUpEventArgs = new OnUpEventArgs
            {
                MotionEvent = e
            };
            OnUp?.Invoke(onUpEventArgs);
            return base.OnSingleTapConfirmed(e);
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var diffY = e2.GetY() - e1.GetY();
            var diffX = e2.GetX() - e1.GetX();

            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                if (!(Math.Abs(diffX) > SWIPE_THRESHOLD) || !(Math.Abs(velocityX) > SWIPE_VELOCITY_THRESHOLD))
                    return base.OnFling(e1, e2, velocityX, velocityY);

                if (diffX > 0)
                {
                    OnSwipeRight?.Invoke(this, null);
                }
                else
                {
                    OnSwipeLeft?.Invoke(this, null);
                }
            }
            else if (Math.Abs(diffY) > SWIPE_THRESHOLD && Math.Abs(velocityY) > SWIPE_VELOCITY_THRESHOLD)
            {
                if (diffY > 0)
                {
                    OnSwipeDown?.Invoke(this, null);
                }
                else
                {
                    OnSwipeUp?.Invoke(this, null);
                }
            }
            return base.OnFling(e1, e2, velocityX, velocityY);
        }
    }

}