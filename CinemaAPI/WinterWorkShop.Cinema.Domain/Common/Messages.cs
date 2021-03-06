﻿namespace WinterWorkShop.Cinema.Domain.Common
{
    public static class Messages
    {
        #region Users

        #endregion

        #region Payments
        public const string PAYMENT_CREATION_ERROR = "Connection error, occured while creating new payment, please try again";
        #endregion

        #region Auditoriums
        public const string AUDITORIUM_DOES_NOT_EXIST = "Auditorium does not exist.";
        public const string AUDITORIUM_GET_ALL_AUDITORIUMS_ERROR = "Error occured while getting all auditoriums, please try again.";
        public const string AUDITORIUM_PROPERTIE_NAME_NOT_VALID = "The auditorium Name cannot be longer than 50 characters.";
        public const string AUDITORIUM_PROPERTIE_SEATROWSNUMBER_NOT_VALID = "The auditorium number of seats rows must be between 1-20.";
        public const string AUDITORIUM_PROPERTIE_SEATNUMBER_NOT_VALID = "The auditorium number of seats number must be between 1-20.";
        public const string AUDITORIUM_CREATION_ERROR = "Error occured while creating new auditorium, please try again.";
        public const string AUDITORIUM_SEATS_CREATION_ERROR = "Error occured while creating seats for auditorium, please try again.";
        public const string AUDITORIUM_SAME_NAME = "Cannot create new auditorium, auditorium with same name alredy exist.";
        public const string AUDITORIUM_UNVALID_CINEMAID = "Cannot create new auditorium, auditorium with given cinemaId does not exist.";
        #endregion

        #region Cinemas
        public const string CINEMA_GET_ALL_CINEMAS_ERROR = "Error occured while getting all cinemas, please try again";
        public const string CINEMA_PROPERTY_NAME_NOT_VALID = "The cinema name can not be longer than 255 characters.";
        public const string CINEMA_CREATION_ERROR = "Error occured while creating new cinema, please try again.";
        public const string CINEMA_DOES_NOT_EXIST = "Cinema does not exist.";
        public const string CINEMA_SAME_NAME = "Cinema with the same name already exists. ";
        #endregion

        #region Movies        
        public const string MOVIE_DOES_NOT_EXIST = "Movie does not exist.";
        public const string MOVIE_DOES_NOT_EXIST_OR_HAVE_PROJECTIONS = "Movie does not exist or it has projections.";
        public const string MOVIE_PROPERTIE_TITLE_NOT_VALID = "The movie title cannot be longer than 50 characters.";
        public const string MOVIE_PROPERTIE_YEAR_NOT_VALID = "The movie year must be between 1895-2100.";
        public const string MOVIE_PROPERTIE_RATING_NOT_VALID = "The movie rating must be between 1-10.";
        public const string MOVIE_CREATION_ERROR = "Error occured while creating new movie, please try again.";
        public const string MOVIE_GET_ALL_CURRENT_MOVIES_ERROR = "Error occured while getting current movies, please try again.";
        public const string MOVIE_GET_BY_ID = "Error occured while getting movie by Id, please try again.";
        public const string MOVIE_GET_ALL_MOVIES_ERROR = "Error occured while getting all movies, please try again.";
        #endregion

        #region Projections
        public const string PROJECTION_DOES_NOT_EXIST = "Projection does not exist. ";
        public const string PROJECTION_GET_ALL_PROJECTIONS_ERROR = "Error occured while getting all projections, please try again.";
        public const string PROJECTION_CREATION_ERROR = "Error occured while creating new projection, please try again.";
        public const string PROJECTIONS_AT_SAME_TIME = "Cannot create new projection, there are projections at same time alredy.";
        public const string PROJECTION_IN_PAST = "Projection time cannot be in past.";
        #endregion

        #region Seats
        public const string SEAT_DOES_NOT_EXIST = "Seat does not exist. ";
        public const string SEAT_GET_ALL_SEATS_ERROR = "Error occured while getting all seats, please try again.";
        #endregion

        #region User
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string USER_CREATION_ERROR = "Error occured while registering a new user, please try again.";
        public const string USER_SAME_USERNAME = "A user with that username already exists, please try again";
        public const string USER_BONUSPOINTS_ERROR = "An error occured while assigning bonus points to the User";
        #endregion

        #region Reservations
        public const string RESERVATION_CREATION_ERROR = "Error occured while creating new reservation. ";
        public const string RESERVATION_DOES_NOT_EXIST = "Reservation with specified parameters does not exist. ";
        public const string RESERVATION_GET_ALL_ERROR = "Error occured while trying to get all reservations. ";
        public const string RESERVATION_GET_ALL_BY_PROJECTIONID_ERROR = "Error occured while attempting to get reservations for the specified projection. ";
        public const string RESERVATION_GET_ALL_BY_PROJECTIONID_USERID_ERROR = "Error occured while attempting to get reservations for the specified user and projection.";
        public const string RESERVATION_GET_ALL_BY_USERID_ERROR = "Error occured while attempting to get reservations for the specified user.";
        #endregion

        #region Tags
        public const string TAG_DOES_NOT_EXIST = "Specified tag does not exist. ";
        public const string TAG_CREATION_ERROR = "An error occured while creating tag. ";
        #endregion

        #region MovieTags
        public const string MOVIETAG_DOES_NOT_EXIST = "The specified MovieTag does not exist. ";
        public const string MOVIETAG_CREATION_ERROR = "Error occured while creating new MovieTag. ";
        #endregion
    }
}
