import React from 'react';
import { withRouter } from 'react-router-dom';
import { FormGroup, FormControl, Button, Container, Row, Col, FormText, } from 'react-bootstrap';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../../appSettings';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  faCouch } from '@fortawesome/free-solid-svg-icons';

class NewCinema extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: '',
            nameError: '',
            auditName: '',
            seatRows: 0,
            numberOfSeats: 0,
            auditNameError: '',
            seatRowsError: '',
            numOfSeatsError: '',
            submitted: false,
            canSubmit: true
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    
    handleChange(e) {
        const { id, value } = e.target;
        this.setState({ [id]: value });
        this.validate(id, value);
    }

    validate(id, value) {
        if (id === 'name') {
            if (value === '') {
                this.setState({nameError: 'Fill in cinema name',
                                canSubmit: false});
            } else {
                this.setState({nameError: '',
                                canSubmit: true});
            }
            if (id === 'auditName') {
                if(value === ''){
                    this.setState({auditNameError: 'Fill in auditorium name',
                                    canSubmit: false})
                } else {
                    this.setState({auditNameError: '',
                                    canSubmit: true});
                }
            } else if (id === 'numberOfSeats') {
                const seatsNum = +value;
                if (seatsNum > 20 || seatsNum < 1) {
                    this.setState({numOfSeatsError: 'Seats number can be in between 1 and 20.',
                                    canSubmit: false})
                } else {
                    this.setState({numOfSeatsError: '',
                                    canSubmit: true});
                }
            } else if (id === 'seatRows') {
                const seatsNum = +value;
                if (seatsNum > 20 || seatsNum < 1) {
                    this.setState({seatRowsError: 'Seats number can be in between 1 and 20.',
                                    canSubmit: false})
                } else {
                    this.setState({seatRowsError: '',
                                    canSubmit: true});
                }
            } 
        }
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ submitted: true });
        const { name } = this.state;
        if (name) {
            this.addCinema();
        } else {
            NotificationManager.error('Please fill in data');
            this.setState({ submitted: false });
        }
    }

    addCinema() {
        const { name, numberOfSeats, seatRows, auditName } = this.state;

        const data = {
            Name: name,
            numberOfSeats: +numberOfSeats,
            seatRows: +seatRows,
            auditName: auditName
        };

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json',
                      'Authorization': 'Bearer ' + localStorage.getItem('jwt')},
            body: JSON.stringify(data)
        };

        fetch(`${serviceConfig.baseURL}/api/cinemas`, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.statusText;
            })
            .then(result => {
                NotificationManager.success('Successfuly added cinema!');
                this.props.history.push(`AllCinemas`);
            })
            .catch(response => {
                NotificationManager.error(response.message || response.statusText);
                this.setState({ submitted: false });
            });
    }

    renderRows(rows, seats) {
        const rowsRendered = [];
        for (let i = 0; i < rows; i++) {
            rowsRendered.push( <tr key={i}>
                {this.renderSeats(seats, i)}
            </tr>);
        }
        return rowsRendered;
    }

    renderSeats(seats, row) {
        let renderedSeats = [];
        for (let i = 0; i < seats; i++) {
            renderedSeats.push(<td id="test" class="rendering-seats" key={'row: ' + row + ', seat: ' + i}><FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch}/></td>);
        }
        return renderedSeats;
    }

    render() {
        const { name, auditName, seatRows, numberOfSeats, submitted, nameError, canSubmit, auditNameError, seatRowsError, numOfSeatsError } = this.state;
        const auditorium = this.renderRows(seatRows, numberOfSeats);
        return (
            <Container>
                <Row>
                    <Col>
                        <h1 className="form-header">Add New Cinema</h1>
                        <form onSubmit={this.handleSubmit}>
                            <FormGroup>
                                <FormControl
                                    id="name"
                                    type="text"
                                    placeholder="Cinema Name"
                                    value={name}
                                    className="add-new-form"
                                    onChange={this.handleChange}
                                />
                                <FormText className="text-danger">{nameError}</FormText>
                                <h1 className="form-header">Add Auditorium For Cinema</h1>
                                <FormControl
                                    id="auditName"
                                    type="text"
                                    placeholder="Auditorium Name"
                                    value={auditName}
                                    onChange={this.handleChange}
                                    className="add-new-form"
                                />
                                 <FormText className="text-danger">{auditNameError}</FormText>
                                 <FormControl
                                    id="seatRows"
                                    className="add-new-form"
                                    type="number"
                                    placeholder="Number Of Rows"
                                    value={seatRows}
                                    onChange={this.handleChange}
                                />
                                <FormText className="text-danger">{seatRowsError}</FormText>
                                <FormControl
                                    id="numberOfSeats"
                                    className="add-new-form"
                                    type="number"
                                    placeholder="Number Of Seats"
                                    value={numberOfSeats}
                                    onChange={this.handleChange}
                                    max="36"
                                />
                                <FormText className="text-danger">{numOfSeatsError}</FormText>
                            </FormGroup>
                            <Button className="btn-add-new" type="submit" disabled={submitted || !canSubmit} block>Add</Button>
                        </form>
                    </Col>
                </Row>
                <Row className="mt-2">
                    <Col className="justify-content-center align-content-center">
                        <h1 className="form-header">Auditorium Preview</h1>
                        <div>
                        <Row className="justify-content-center">
                            <table className="table-cinema-auditorium">
                            <tbody>
                            {auditorium}
                            </tbody>
                            </table>
                        </Row>
                        <Row className="justify-content-center mb-4">
                            <div className="text-center text-white font-weight-bold cinema-screen">
                                CINEMA SCREEN
                            </div>
                        </Row>
                        </div>
                    </Col>
                </Row>
            </Container>
        );
    }
}

export default withRouter(NewCinema);