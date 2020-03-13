import React, { Component } from 'react';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../../appSettings';
import { Row, Table } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faTrash, faAnchor, faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import Spinner from '../../Spinner';
import './../../../index.css'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


class TopTenMovies extends Component {
  constructor(props) {
    super(props);
    this.state = {
      movies: [],
      filteredMoviesByYear: [],
      title: '',
      year: 0,
      id: '',
      rating: 0,
      tags: [],
      current: false,
      titleError: '',
      yearError: '',
      submitted: false,
      isLoading: true,
      selectedYear: false
    };
  }

  componentDidMount() {
    this.getProjections();
  }

  getMovie(movieId) {
    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/movies/` + movieId, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            title: data.title,
            year: data.year,
            rating: Math.round(data.rating) + '',
            current: data.current + '',
            id: data.id
          });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }


  getProjections() {
    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Movies/top`, requestOptions)
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
        this.setState({ submitted: false });
      });
  }

  getTagsByMovieId(e, movieId) {

    e.preventDefault();

    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('jwt')
        }
    };

    fetch(`${serviceConfig.baseURL}/api/tags/getbymovieid/${movieId}`, requestOptions)
        .then(response => {
            if (!response.ok) {
                return Promise.reject(response);
            }
            return response.json();
        })
        .then(data => {
            if (data) {
                this.setState({ tags: data });
                this.showTags();
            }
        })
        .catch(response => {
            NotificationManager.error(response.message || response.statusText);
            this.setState({ submitted: false });
        });
}

showTags() {
    const { listOfTags } = this.state;
    this.setState({ listOfTags: [] });
    this.state.tags.map(tag => {
        this.state.listOfTags.push(tag.name);
    });
    var list = ' | ';
    for (var i = 0; i < (this.state.listOfTags.length); i++) {
        list += this.state.listOfTags[i] + ' | ';
    }
    toast.info(list, {
        position: toast.POSITION.BOTTOM_CENTER,
        className: "toast-class"
    });

}

  fillTableWithDaata() {
    
    if (this.state.filteredMoviesByYear.length > 0) {
      return this.state.filteredMoviesByYear.map(filteredMovie => {
        return <tr key={filteredMovie.id}>
          <td className="text-center cursor-pointer" onClick={(e) => this.getTagsByMovieId(e, filteredMovie.id)} ><FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faInfoCircle} /></td>
          <td>{filteredMovie.title}</td>
          <td>{filteredMovie.year}</td>
          <td>{Math.round(filteredMovie.rating)}/10</td>
        </tr>
      })
    }
    else {
      if (this.state.selectedYear) {
        this.setState({selectedYear: false});
        NotificationManager.error("Movies with selected year don't exist.");
      }
      return this.state.movies.map(movie => {
        return <tr key={movie.id}>
          <td className="text-center cursor-pointer" onClick={(e) => this.getTagsByMovieId(e, movie.id)} ><FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faInfoCircle} /></td>
          <td>{movie.title}</td>
          <td>{movie.year}</td>
          <td>{Math.round(movie.rating)}/10</td>
        </tr>
      })
    }
  }


  showYears() {
    let yearOptions = [];
    for (var i = 1960; i <= 2100; i++) {
      yearOptions.push(<option value={i} name="selectedYear">{i}</option>)
    }
    return yearOptions;
  }

  getTopTenMoviesByYear(year) {


    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    this.setState({ isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/movies/topbyyear/${year}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ filteredMoviesByYear: data, isLoading: false, selectedYear: true });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ isLoading: false });
      });

  }

  render() {
    const { isLoading } = this.state;
    const rowsData = this.fillTableWithDaata();
    const table = (<Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Tags</th>
          <th>Title</th>
          <th>Year</th>
          <th>Rating</th>
        </tr>
      </thead>
      <tbody>
        {rowsData}
      </tbody>
    </Table>);
    const showTable = isLoading ? <Spinner></Spinner> : table;
    const showYears = this.showYears();

    return (
      <React.Fragment>
        <Row className="no-gutters pt-2">
          <h1 className="form-header form-heading">Top 10 Movies</h1>
        </Row>
        <Row className="year-filter">
          <span class="filter-heading">Filter by:</span>
          <select onChange={(e) => this.getTopTenMoviesByYear(e.target.value)} name="movieYear" id="movieYear" class="select-dropdown" min="1900" max="2100">
            <option value="" selected>Year</option>
            {showYears}
          </select>
        </Row>
        <Row className="no-gutters pr-5 pl-5">
          {showTable}
        </Row>
      </React.Fragment>
    );
  }
}

export default TopTenMovies;