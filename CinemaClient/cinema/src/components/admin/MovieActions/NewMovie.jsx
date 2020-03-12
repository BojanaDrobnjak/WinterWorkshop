import React from 'react';
import { withRouter } from 'react-router-dom';
import { FormGroup, FormControl, Button, Container, Row, Col, FormText, } from 'react-bootstrap';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../../appSettings';
import { YearPicker } from 'react-dropdown-date';

class NewMovie extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            title: '',
            year: 0,
            rating: '',
            current: false,
            titleError: '',
            submitted: false,
            canSubmit: true,
            tags: '',
            bannerUrl: ''
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleYearChange = this.handleYearChange.bind(this);
    }

    handleChange(e) {
        const { id, value } = e.target;
        this.setState({ [id]: value });
        this.validate(id, value);
    }

    handleTagsChange(e) {
        this.setState({ tags: e.target.value });
    }

    handleBannerUrlChange(e) {
        this.setState({ bannerUrl: e.target.value });
    }

    validate(id, value) {
        if (id === 'title') {
            if (value === '') {
                this.setState({
                    titleError: 'Fill in movie title',
                    canSubmit: false
                });
            } else {
                this.setState({
                    titleError: '',
                    canSubmit: true
                });
            }
        }

        if (id === 'year') {
            const yearNum = +value;
            if (!value || value === '' || (yearNum < 1895 || yearNum > 2100)) {
                this.setState({ yearError: 'Please chose valid year' });
            } else {
                this.setState({ yearError: '' });
            }
        }
    }

    handleSubmit(e) {
        e.preventDefault();
        let splitTags = this.state.tags.split(",");

        this.setState({ submitted: true });
        const { title, year, rating } = this.state;
        if (title && year && rating && splitTags[0] !== "") {
            this.addMovie(splitTags);
        } else {
            NotificationManager.error('Please fill in data');
            this.setState({ submitted: false });
        }
    }

    handleYearChange(year) {
        this.setState({ year: year });
        this.validate('year', year);
    }

    addMovie(splitTags) {
        const { title, year, current, rating, bannerUrl } = this.state;

        const data = {
            Title: title,
            Year: +year,
            Current: current === "true",
            Rating: +rating,
            Tags: splitTags,
            BannerUrl : bannerUrl
        };

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            },
            body: JSON.stringify(data)
        };

        fetch(`${serviceConfig.baseURL}/api/movies`, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.statusText;
            })
            .then(result => {
                NotificationManager.success('Successfuly added movie!');
                this.props.history.push(`AllMovies`);
            })
            .catch(response => {
                NotificationManager.error(response.message || response.statusText);
                this.setState({ submitted: false });
            });
    }

    render() {
        const { bannerUrl, tags, title, year, current, rating, submitted, titleError, yearError, canSubmit } = this.state;
        return (
            <Container>
                <Row>
                    <Col>
                        <h1 className="form-header">Add New Movie</h1>
                        <form onSubmit={this.handleSubmit}>
                            <FormGroup>
                                <FormControl
                                    id="title"
                                    type="text"
                                    placeholder="Movie Title"
                                    value={title}
                                    onChange={this.handleChange}
                                    className="add-new-form"
                                />
                                <FormText className="text-danger">{titleError}</FormText>
                            </FormGroup>
                            <FormGroup>
                                <YearPicker
                                    defaultValue={'Select Movie Year'}
                                    start={1895}
                                    end={2100}
                                    reverse
                                    required={true}
                                    disabled={false}
                                    value={year}
                                    onChange={(year) => {
                                        this.handleYearChange(year);
                                    }}
                                    id={'year'}
                                    name={'year'}
                                    classes={'form-control add-new-form'}
                                    optionClasses={'option classes'}
                                />
                                <FormText className="text-danger">{yearError}</FormText>
                            </FormGroup>
                            <FormGroup>
                                <FormControl as="select" className="add-new-form" placeholder="Rating" id="rating" value={rating} onChange={this.handleChange}>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                </FormControl>
                            </FormGroup>
                            <FormGroup>
                                <FormControl className="add-new-form" as="select" placeholder="Current" id="current" value={current} onChange={this.handleChange}>
                                    <option value="true">Current</option>
                                    <option value="false">Not Current</option>
                                </FormControl>
                            </FormGroup>
                            <FormControl
                                id="tags"
                                type="text"
                                placeholder="Movie Tags"
                                value={tags}
                                onChange={(e) => {
                                    this.handleTagsChange(e);
                                }}
                                className="add-new-form"
                            />
                            <FormControl
                                id="bannerUrl"
                                type="text"
                                placeholder="Banner Url"
                                value={bannerUrl}
                                onChange={(e) => {
                                    this.handleBannerUrlChange(e);
                                }}
                                className="add-new-form"
                            />
                            <FormText className="text-danger">{titleError}</FormText>
                            <Button className="btn-add-new" type="submit" disabled={submitted || !canSubmit} block>Add</Button>
                        </form>
                    </Col>
                </Row>
            </Container>
        );
    }
}

export default withRouter(NewMovie);