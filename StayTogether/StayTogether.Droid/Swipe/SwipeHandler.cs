using System;
using Android.Content;
using Android.Views;

namespace StayTogether.Droid.Swipe
{
    public class SwipeHandler
    {
        private readonly GestureDetector _gestureDetector;
        private readonly GestureListener _gestureListener;
        private static readonly int SWIPE_THRESHOLD = 100;
        private static readonly int SWIPE_VELOCITY_THRESHOLD = 100;

        public event EventHandler OnSwipeDown;
        public event EventHandler OnSwipeUp;
        public event EventHandler OnSwipeLeft;
        public event EventHandler OnSwipeRight;

        public SwipeHandler(Context context)
        {
            _gestureListener = new GestureListener();
            _gestureListener.OnSwipeDown += (sender, args) => { OnSwipeDown?.Invoke(this, null); };
            _gestureListener.OnSwipeUp += (sender, args) => { OnSwipeUp?.Invoke(this, null); };
            _gestureListener.OnSwipeLeft += (sender, args) => { OnSwipeLeft?.Invoke(this, null); };
            _gestureListener.OnSwipeRight += (sender, args) => { OnSwipeRight?.Invoke(this, null); };

            _gestureDetector = new GestureDetector(context, _gestureListener);
        }

        public bool OnTouch(MotionEvent e)
        {
            return _gestureDetector.OnTouchEvent(e);
        }

        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            public event EventHandler OnSwipeDown;
            public event EventHandler OnSwipeUp;
            public event EventHandler OnSwipeLeft;
            public event EventHandler OnSwipeRight;

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

        public void Dispose()
        {
            _gestureDetector.Dispose();
        }
    }
}