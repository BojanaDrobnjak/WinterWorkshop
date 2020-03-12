import React, { Component } from 'react';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../../appSettings';
import { Row, Table, Form, FormControl, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faTrash, faAnchor, faInfoCircle, faLightbulb } from '@fortawesome/free-solid-svg-icons';
import Spinner from '../../Spinner';
import './../../../index.css';
import { isAdmin, isSuperUser, isUser, isGuest } from './../../helpers/authCheck';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

class ShowAllMovies extends Component {
    constructor(props) {
        super(props);
        this.state = {
            movies: [],
            tags: [],
            title: '',
            year: 0,
            id: '',
            rating: 0,
            current: false,
            tag: '',
            listOfTags: [],
            titleError: '',
            yearError: '',
            submitted: false,
            isLoading: true
        };
        toast.configure();
        this.editMovie = this.editMovie.bind(this);
        this.removeMovie = this.removeMovie.bind(this);
        this.changeCurrent = this.changeCurrent.bind(this);
        this.searchMovie = this.searchMovie.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.showTags = this.showTags.bind(this);
    }

    userShouldSeeWholeTable;
    shouldUserSeeWholeTable() {
        if (this.userShouldSeeWholeTable === undefined) {
            this.userShouldSeeWholeTable = (!isGuest() && !isUser());
        }
        return this.userShouldSeeWholeTable;
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

    changeCurrent(e, id) {
        e.preventDefault();
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            }
        };

        fetch(`${serviceConfig.baseURL}/api/movies/changecurrent/${id}`, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.statusText;
            })
            .then(result => {
                NotificationManager.success('Successfuly changed current status for movie!');
                const newState = this.state.movies.filter(movie => {
                    return movie.id !== id;
                })
                this.setState({ movies: newState });
                setTimeout(() => { window.location.reload(); }, 2000);

            })
            .catch(()=> {
                NotificationManager.error("This movie has projections!");
                this.setState({ submitted: false });
            });
    }


    getProjections() {
        if (isAdmin() === true || isSuperUser() === true) {
            const requestOptions = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('jwt')
                }
            };

            this.setState({ isLoading: true });
            fetch(`${serviceConfig.baseURL}/api/movies/all`, requestOptions)
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
        else {
            const requestOptions = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('jwt')
                }
            };

            this.setState({ isLoading: true });
            fetch(`${serviceConfig.baseURL}/api/movies/current`, requestOptions)
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
    }

    removeMovie(id) {
        const requestOptions = {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            }
        };

        fetch(`${serviceConfig.baseURL}/api/movies/${id}`, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.statusText;
            })
            .then(result => {
                NotificationManager.success('Successfuly removed movie!');
                const newState = this.state.movies.filter(movie => {
                    return movie.id !== id;
                })
                this.setState({ movies: newState });
            })
            .catch(response => {
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

        return this.state.movies.map(movie => {

            return <tr key={movie.id}>
                <td className="text-center cursor-pointer" onClick={(e) => this.getTagsByMovieId(e, movie.id)} ><FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faInfoCircle} /></td>
                <td>{movie.title}</td>
                <td>{movie.year}</td>
                <td>{Math.round(movie.rating)}/10</td>

                {this.shouldUserSeeWholeTable() && (<td>{movie.current ? 'Yes' : 'No'}</td>)}
                {this.shouldUserSeeWholeTable() && (<td className="text-center cursor-pointer" onClick={() => this.editMovie(movie.id)}><FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} /></td>)}
                {this.shouldUserSeeWholeTable() && (<td className="text-center cursor-pointer" onClick={() => this.removeMovie(movie.id)}><FontAwesomeIcon className="text-danger mr-2 fa-1x" icon={faTrash} /></td>)}
                {this.shouldUserSeeWholeTable() && (<td className="text-center cursor-pointer" onClick={(e) => this.changeCurrent(e, movie.id)}><FontAwesomeIcon className={movie.current ? 'text-warning mr-2 fa-1x' : 'text-secondary mr-2 fa-1x'} icon={faLightbulb} /></td>)}

            </tr>
        })
    }

    editMovie(id) {
        this.props.history.push(`editmovie/${id}`);
    }

    handleChange(e) {
        const { id, value } = e.target;
        this.setState({ [id]: value });
    }

    handleSubmit(e) {
        e.preventDefault();

        var data = new FormData(e.currentTarget);

        var queryParts = [];
        var entries = data.entries();

        for (var pair of entries) {
            queryParts.push(encodeURIComponent(pair[0]) + "=" + encodeURIComponent(pair[1]));
        }
        var query = queryParts.join("&");
        var loc = window.location;
        var url = loc.protocol + '//' + loc.host + loc.pathname + '?' + query;

        let tag = url.split("=")[1];

        this.setState({ submitted: true });
        if (tag) {
            this.searchMovie(tag);
        } else {
            NotificationManager.error('Please type type something in search bar to search for movies.');
            this.setState({ submitted: false });
        }
    }

    searchMovie(tag) {

        const requestOptions = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            }
        };

        this.setState({ isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Movies/bytag/${tag}`, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then(data => {
                if (data) {
                    this.setState({ movies: data, isLoading: false });
                } else {
                    const { title } = this.state;
                    const requestOptions = {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + localStorage.getItem('jwt')
                        }
                    };

                    this.setState({ isLoading: true });
                    fetch(`${serviceConfig.baseURL}/api/Movies/bytitle/${title}`, requestOptions)
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
                        });
                }
            })
            .catch(response => {
                this.setState({ isLoading: false });
                NotificationManager.error("Movie doesn't exists.");
                this.setState({ submitted: false });
            });

    }

    inputValue;
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
                    {this.shouldUserSeeWholeTable() && (<th>Is Current</th>)}
                    {this.shouldUserSeeWholeTable() && (<th></th>)}
                    {this.shouldUserSeeWholeTable() && (<th></th>)}
                </tr>
            </thead>
            <tbody>
                {rowsData}
            </tbody>
        </Table>);
        const showTable = isLoading ? <Spinner></Spinner> : table;

        return (
            <React.Fragment>
                <Row className="no-gutters pt-2">
                    <h1 className="form-header form-heading">All Movies</h1>
                </Row>
                <Row>
                    <form onSubmit={(e) => this.handleSubmit(e)} class="form-inline search-field md-form mr-auto mb-4 search-form search-form-second">
                        <input class="mr-sm-2 search-bar" id="tag" type="text" placeholder="Search" name="inputValue" value={this.inputValue} aria-label="Search" />
                        <button class="btn-search" type="submit">Search</button>
                    </form>
                </Row>
                <Row className="no-gutters pr-5 pl-5">
                    {showTable}
                </Row>
            </React.Fragment>
        );
    }
}

export default ShowAllMovies;