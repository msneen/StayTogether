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
        private SwipeGestureListener _swipeGestureListener;

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
            _swipeGestureListener = new SwipeGestureListener();
            _swipeGestureListener.OnSwipeDown += (sender, args) => { OnSwipeDown?.Invoke(this, null); };
            _swipeGestureListener.OnSwipeUp += (sender, args) => { OnSwipeUp?.Invoke(this, null); };
            _swipeGestureListener.OnSwipeLeft += (sender, args) => { OnSwipeLeft?.Invoke(this, null); };
            _swipeGestureListener.OnSwipeRight += (sender, args) => { OnSwipeRight?.Invoke(this, null); };
            _swipeGestureListener.OnUp += args => { OnUp?.Invoke(args); };

            _gestureDetector = new GestureDetector(_context, _swipeGestureListener);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return _gestureDetector.OnTouchEvent(e);
        }
       
        public bool OnTouch(MotionEvent e)
        {
            return _gestureDetector.OnTouchEvent(e);
        }


        
        public bool OnDown(MotionEvent e)
        {
            return _swipeGestureListener.OnDown(e);
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return _swipeGestureListener.OnFling(e1, e2, velocityX, velocityY);
        }

        public void OnLongPress(MotionEvent e)
        {
            //throw new NotImplementedException();
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
           
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return true;
        }
        
    }
}