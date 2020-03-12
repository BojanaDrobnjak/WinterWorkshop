import React, { Component } from 'react';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../appSettings';
import { withRouter } from 'react-router-dom';
import { Container, Row, Col, Card, Button, Table } from 'react-bootstrap';
import './../../index.css';

class Projection extends Component {
  constructor(props) {
    super(props);
    this.state = {
      movies: [],
      cinemas: [],
      auditoriums: [],
      filteredAuditoriums: [],
      filteredMovies: [],
      filteredProjections: [],
      cinema: '',
      cinemaId: '',
      auditoriumId: '',
      movieId: '',
      dateTime: '',
      name: '',
      title: '',
      year: 0,
      id: '',
      rating: 0,
      current: false,
      tag: '',
      titleError: '',
      yearError: '',
      submitted: false,
      isLoading: true,
      selectedCinema: false,
      selectedAuditorium: false,
      selectedMovie: false,
      selectedDate: false,
      date: new Date()
    };
    this.getRoundedRating = this.getRoundedRating.bind(this);
    this.navigateToProjectionDetails = this.navigateToProjectionDetails.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getCurrentFilteredMoviesAndProjections = this.getCurrentFilteredMoviesAndProjections.bind(this);

  }

  componentDidMount() {
    this.getCurrentMoviesAndProjections();
    this.getAllCinemas();
    this.getAllAuditoriums();
  }

  //   handleChange(e) {
  //     const { id, value } = e.target;
  //     this.setState({ [id]: value });
  // }

  handleSubmit(e) {
    e.preventDefault();
    this.setState({ submitted: true });
    const { cinemaId, auditoriumId, movieId, dateTime } = this.state;

    if (cinemaId || auditoriumId || movieId || dateTime) {
      this.getCurrentFilteredMoviesAndProjections();
      console.log("uspesno");

    } else {
      NotificationManager.error('Not found.');
      this.setState({ submitted: false });
    }

  }

  getRoundedRating(rating) {
    const result = Math.round(rating);
    return <span className="float-right">Rating: {result}/10</span>
  }

  navigateToProjectionDetails(id, movieId) {
    this.props.history.push('projectiondetails/' + id + "/" + movieId);

  }
  getCurrentMoviesAndProjections() {

    this.setState({ submitted: false });

    console.log("ucitana defaultna lista");

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      },
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/movies/currentMoviesAndProjections`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ movies: data, isLoading: false });
        }
      })
      .catch(response => {
        this.setState({ isLoading: false });
        NotificationManager.error(response.message || response.statusText);
        // this.setState({ submitted: false });
      });

  }

  getCurrentFilteredMoviesAndProjections() {

    console.log("doslo do : getCurrentFilteredMoviesAndProjections");

    const { cinemaId, auditoriumId, movieId, dateTime } = this.state;
    // var cinemaId = (window.location.search).split('&')[0].slice(10);
    // var auditoriumId = (window.location.search).split('&')[1].slice(13);
    // var movieId = (window.location.search).split('&')[2].slice(8);
    // var dateTime = (window.location.search).split('&')[3].slice(9);
    console.log("cinemaId: " + cinemaId + ", auditoriumId: " + auditoriumId + ", movieId: " + movieId + ", dateTime: " + dateTime);

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      },
    };

    this.setState({ isLoading: true });
    let query = "";
    if(cinemaId){
      query = `cinemaId=${cinemaId}`;
      console.log("cinemaId", cinemaId, query);
    }
    if(auditoriumId){
      query += `${query.length ? "&" : ""}auditoriumId=${auditoriumId}`;
      console.log("auditoriumId", auditoriumId, query);
    }
    if(movieId){
      query += `${query.length ? "&" : ""}movieId=${movieId}`;
      console.log("movieId", movieId, query);
    }
    if(dateTime){
      query += `${query.length ? "&" : ""}dateTime=${dateTime}`;
      console.log("dateTime", dateTime, query);
    }
    if(query.length){
      query = `?${query}`;
    }
    fetch(`${serviceConfig.baseURL}/api/projections/filter${query}`, requestOptions)
      .then(response => {

        if (!response.ok) {
          console.log("nije dobar response");

          return Promise.reject(response);
        }

        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ filteredProjections: data, isLoading: false });
          console.log("fetch prosao");
         console.log("filtrirani podaci: ", data);

        }
      })
      .catch(response => {
        this.setState({ isLoading: false });
        NotificationManager.error(response.message || response.statusText);
      });

  }

  getAllAuditoriums() {

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ auditoriums: data, isLoading: false });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ isLoading: false });
      });

  }

  getAllCinemas() {

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Cinemas/all`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            cinemas: data,
            isLoading: false
          });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ isLoading: false });
      });

  }

  fillFilterWithMovies() {

    const checkIfSelected = this.state.selectedAuditorium;
    if (checkIfSelected) {
      return this.state.filteredMovies.map(movie => {
        return <option value={movie.id} key={movie.id}>{movie.title}</option>
      });
    } else {
      return this.state.movies.map(movie => {
        return <option value={movie.id} key={movie.id}>{movie.title}</option>
      });
    }
  }

  fillFilterWithCinemas() {

    return this.state.cinemas.map(cinema => {
      return <option value={cinema.id} >{cinema.name}</option>
    });
  }

  fillFilterWithAuditoriums() {

    const checkIfSelected = this.state.selectedCinema;
    if (checkIfSelected) {
      return this.state.filteredAuditoriums.map(auditorium => {
        return <option value={auditorium.id}>{auditorium.name}</option>
      });
    } else {
      return this.state.auditoriums.map(auditorium => {
        return <option value={auditorium.id}>{auditorium.name}</option>
      });
    }
  }

  fillTableWithData() {

    var checkIfSelectedCinema = this.state.selectedCinema;
    var checkIfSelectedAuditorium = this.state.selectedAuditorium;
    var checkIfSelectedMovie = this.state.selectedMovie;
    var checkIfSelectedDate = this.state.selectedDate;

    console.log(checkIfSelectedCinema, checkIfSelectedAuditorium, checkIfSelectedMovie, checkIfSelectedDate);

    return this.state.movies.map(movie => {
      var pr = movie.projections;
      var proj = pr.map((projection, index) => {
        return <Button key={projection.id} onClick={() => this.navigateToProjectionDetails(projection.id, movie.id)} className="btn-projection-time">{projection.projectionTime.slice(11,16)}h</Button>;
      });

      return <Card.Body>
        <Card.Title><span className="card-title-font">{movie.title}</span> {this.getRoundedRating(movie.rating)}</Card.Title>
        <hr />
        <Card.Subtitle className="mb-2 text-muted">Year of production: {movie.year}</Card.Subtitle>
        <hr />
        <Card.Text>
          <span className="mb-2 font-weight-bold">
            Projection times:
        </span>
        </Card.Text>
        {proj}
      </Card.Body>
    });

  }

  fillTableWithFilteredProjections() {

    console.log("table with filtered projections");
    console.log("lista filter", this.state.filteredProjections);

    return this.state.filteredProjections.map(filteredProj => {
      return <Card.Body>
        <Card.Title><span className="card-title-font">{filteredProj.movieTitle} - {filteredProj.auditoriumName}</span> {this.getRoundedRating(filteredProj.movieRating)}</Card.Title>
        <hr />
    <Card.Subtitle className="mb-2 text-muted">Year of production: {filteredProj.movieYear}</Card.Subtitle>
        <hr />
        <Card.Text>
          <span className="mb-2 font-weight-bold">
            Projection times:
        </span>
        </Card.Text>
        <Button key={filteredProj.id} onClick={() => this.navigateToProjectionDetails(filteredProj.id, filteredProj.movieId)} className="btn-projection-time">{filteredProj.projectionTime.slice(11,16)}h</Button>
      </Card.Body>
    })
  }


  getAuditoriumsBySelectedCinema(selectedCinema) {

    this.setState({ cinemaId: selectedCinema });

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Auditoriums/bycinemaid/${selectedCinema}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ filteredAuditoriums: data, isLoading: false, selectedCinema: true });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ isLoading: false });
      });
    var enableAudit = document.getElementById('auditorium').disabled = false;
  }

  getMoviesBySelectedAuditorium(selectedAuditorium) {

    this.setState({ auditoriumId: selectedAuditorium })
    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Movies/byauditoriumid/${selectedAuditorium}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ filteredMovies: data, isLoading: false, selectedAuditorium: true });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ isLoading: false });
      });
    var enableMovies = document.getElementById('movie').disabled = false;
    var enableMovies = document.getElementById('date').disabled = false;

  }

  checkIfFiltered() {

    if (this.state.submitted) {
      return this.fillTableWithFilteredProjections();
    } else {
      return this.fillTableWithData();
    }
  }

  onDateChange = date => this.setState({ date })

  render() {
    const cinemaFilter = this.fillFilterWithCinemas();
    const movieFilter = this.fillFilterWithMovies();
    const auditoriumFilter = this.fillFilterWithAuditoriums();
    const { name } = this.state;
    const checkRowsData = this.checkIfFiltered();
    return (
      <Container>
        <h1>Current projections</h1>
        <form id="name" name={name} onSubmit={this.handleSubmit} className="filter">
          <span class="filter-heading">Filter by:</span>
          <select onChange={(e) => this.getAuditoriumsBySelectedCinema(e.target.value)} name="cinemaId" id="cinema" class="select-dropdown">
            <option value="" selected>Cinema</option>
            {cinemaFilter}
          </select>
          <select onChange={(e) => this.getMoviesBySelectedAuditorium(e.target.value)} name="auditoriumId" id="auditorium" class="select-dropdown" disabled>
            <option value="" selected>Auditorium</option>
            {auditoriumFilter}
          </select>
          <select onChange={(e) => this.setState({ selectedMovie: true, movieId: e.target.value })} name="movieId" id="movie" class="select-dropdown" disabled>
            <option value="" selected>Movie</option>
            {movieFilter}
          </select>
          <input onChange={(e) => this.setState({ selectedDate: true, dateTime: e.target.value })} name="dateTime" type="date" id="date" class="input-date select-dropdown" disabled />
          <button id="filter-button" class="btn-search" type="submit" onClick={() => this.setState({ submitted: true })} >Submit</button>
        </form>
        <Row className="justify-content-center">
          <Col>
            <Card className="card-width">
              {checkRowsData}
            </Card>
          </Col>
        </Row>
      </Container>
    );
  }
}

export default withRouter(Projection);