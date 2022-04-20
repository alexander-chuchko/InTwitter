using System;
using System.Collections.Generic;
using System.Text;

namespace InTwitter
{
    public static class Constants
    {
        // Sign Up
        public const string NEW_USER_DATA = "NEW_USER_DATA";
        public const string NEW_USER_EMAIL = "NEW_USER_EMAIL";

        public const string TWEET_MARK_UPDATED = "TWEET_MARK_UPDATED";
        public const string TWEET_LIKE_UPDATED = "TWEET_LIKE_UPDATED";

        // Addpost
        public const int MAX_LENGHT_TWEET = 250;

        //Pressed Button
        public const string PRESSED_BUTTON = "PRESSED";
        public const string LONG_PRESSED_BUTTON = "LONG_PRESSED";
        public const string RELEASE_BUTTON = "RELEASE";

        //Tap Command Page of Camera
        public const string TAP_FLIP_CAMERA = "TAP_FLIP_CAMERA";
        public const string TAP_POST = "TAP_POST";

        //Duration of viewing items of stories (in seconds)
        public const double DURATION_PREVIEW_PICTURE = 5.0;
        public const double DURATION_PREVIEW_VIDEO = 30.0;
        public const double LIFETIME_OF_POSTIN_STORY = 60.0;

        public const double PROGRESS_BAR_SPEED = 10.0;
        public const double PROGRESS_BAR_MAX_VALUE = 100.0;
        public const double PROGRESS_BAR_MIN_VALUE = 0.0;

        //Number of displayed characters of the name
        public const int NUMBER_OF_CHARACTERS_IN_THE_NAME = 4;

        //Description of keys for command processing in the viewmodel
        public const string CLOSING_PAGE = "CLOSING_PAGE";
        public const string GO_TO_STORAGE_MEDIA = "GO_TO_STORAGE_MEDIA";
        public const string GO_TO_CAMERA_PAGE = "GO_TO_ADD_STORIES_PAGE";
        public const string REMOVE_ITEM_FROM_STORIES = "REMOVE_ITEM_FROM_STORIES";
        public const string TRANSITION_TO_VIDEO_RESOURCES = "VIDEO";
        public const string TRANSITION_TO_PICTURE_RESOURCES = "PICTURE";

        //Id
        public const string ID_USER = "ID_USER";

        //Constants for ImageSaver
        public const string PORTRAIT_ORIENTATION_TAG = "6";

        //Video player constants
        public const string STOP_PLAY = "STOP_PLAY";
        public const string PAUSE_PLAY = "PAUSE_PLAY";
        public const string PLAY = "PLAY";
    }
}