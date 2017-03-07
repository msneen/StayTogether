using System;
using Android.Content;
using Android.Views;

namespace StayTogether.Droid.Swipe
{
    public delegate void OnUpEventHandler(OnUpEventArgs e);

    public class OnUpEventArgs : EventArgs
    {
        public MotionEvent MotionEvent;
    }

    public class SwipeHandler : Java.Lang.Object, View.IOnTouchListener, GestureDetector.IOnGestureListener
    {
        private GestureDetector _gestureDetector;
        private Context _context;

        public event EventHandler OnSwipeDown;
        public event EventHandler OnSwipeUp;
        public event EventHandler OnSwipeLeft;
        public event EventHandler OnSwipeRight;
        public event OnUpEventHandler OnUp;

        public void SetContext(Context context)
        {
            _context = context;
            Initialize();
        }

        public SwipeHandler()
        {
                
        }

        public SwipeHandler(Context context)
        {         
            SetContext(context);
        }

        private void Initialize()
        {
            var swipeGestureListener = new SwipeGestureListener();
            swipeGestureListener.OnSwipeDown += (sender, args) => { OnSwipeDown?.Invoke(this, null); };
            swipeGestureListener.OnSwipeUp += (sender, args) => { OnSwipeUp?.Invoke(this, null); };
            swipeGestureListener.OnSwipeLeft += (sender, args) => { OnSwipeLeft?.Invoke(this, null); };
            swipeGestureListener.OnSwipeRight += (sender, args) => { OnSwipeRight?.Invoke(this, null); };
            swipeGestureListener.OnUp += args => { OnUp?.Invoke(args); };

            _gestureDetector = new GestureDetector(_context, swipeGestureListener);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return _gestureDetector.OnTouchEvent(e);
        }
       
        public bool OnTouch(MotionEvent e)
        {
            //if (e.Action == MotionEventActions.Up)
            //{
            //   var onUpEventArgs = new OnUpEventArgs
            //   {
            //       MotionEvent = e
            //   };
            //    OnUp?.Invoke(onUpEventArgs);
            //    return true;
            //}
            return _gestureDetector.OnTouchEvent(e);
        }

        public void Dispose()
        {
            _gestureDetector.Dispose();
            base.Dispose();
        }

        public IntPtr Handle { get; }

        
        public bool OnDown(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            throw new NotImplementedException();
        }

        public void OnLongPress(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            throw new NotImplementedException();
        }

        public void OnShowPress(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            throw new NotImplementedException();
        }
        
    }
}