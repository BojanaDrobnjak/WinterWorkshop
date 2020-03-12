import React, { Component, useState } from 'react';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../appSettings';
import { Container, Row, Col, Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faShoppingCart, faCouch } from '@fortawesome/free-solid-svg-icons';
import { getUserName, getRole } from '../helpers/authCheck';


class ProjectionDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
      projections: [],
      movies: [],
      seats: [],
      slicedTime: '',
      moviesId: '',
      auditId: 0,
      maxRow: 0,
      maxNumberOfRow: 0,
      inc: 0,
      allSeats: [],
      inciD: 0,
      clicked: false,
      projectionId: '',
      currentReservationSeats: [],
      reservedSeats: [],
      userId: ''
    };

  }
  allButtons = undefined;


  componentDidMount() {
    this.getMovie();
    this.getProjection();
    this.getUserByUsername();
  }

  getProjection() {

    var idFromUrl = (window.location.pathname).split("/");
    var id = idFromUrl[3];

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/projections/byprojectionid/` + id, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            projections: data,
            auditId: data.auditoriumId,
            slicedTime: data.projectionTime.slice(11, 16),
            moviesId: data.movieId
          });

          this.getReservedSeats(requestOptions, id);
          this.getSeatsForAuditorium(data.auditoriumId);
          this.getSeats(data.auditoriumId);

        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  getReservedSeats(requestOptions, id) {
    const reservedSeats = fetch(`${serviceConfig.baseURL}/api/reservations/getbyprojectionid/${id}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            reservedSeats: data
          });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  getSeatsForAuditorium(auditId) {
    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/seats/numberofseats/` + auditId, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            seats: data,
            maxRow: data.maxRow,
            maxNumberOfRow: data.maxNumber,
          });

        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  getUserByUsername() {
    let ussName = getUserName();

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/users/byusername/` + ussName, requestOptions)
      .then(response => {
        if (!response.ok) {
          return;
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({ userId: data.id });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });

  }

  getSeats(auditId) {
    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/seats/byauditoriumid/` + auditId, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            allSeats: data
          });
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  getMovie() {
    var id = (window.location.pathname).split("/")[4];
    const requestOptions = {
      method: 'GET',
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
        return response.json();
      })
      .then(data => {
        if (data) {
          this.setState({
            movies: data
          });

        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  tryReservation(e) {
    e.preventDefault();
    const requestOptions = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      },
      body: ''
    };

    fetch(`${serviceConfig.baseURL}/api/levi9payment`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then(result => {
        this.makeReservation(e);
      })
      .catch(response => {
        NotificationManager.warning('Insufficient founds.');
        this.setState({ submitted: false });
      });

  }

  makeReservation(e) {
    e.preventDefault();

    if (getRole() === "user" || getRole() === "superUser" || getRole() === "admin") {

      var idFromUrl = (window.location.pathname).split("/");
      var projectionId = idFromUrl[3];

      const { currentReservationSeats } = this.state;
      
      const data = {
        projectionId: projectionId,
        seatIds: currentReservationSeats,
        userId: this.state.userId
      };

      const requestOptions = {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + localStorage.getItem('jwt')
        },
        body: JSON.stringify(data)
      };

      fetch(`${serviceConfig.baseURL}/api/reservations`, requestOptions)
        .then(response => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.statusText;
        })
        .then(result => {
          NotificationManager.success('Your reservation has been made successfully!');
          setTimeout(() => { window.location.reload(); }, 2000);
        })
        .catch(response => {
          NotificationManager.error(response.message || response.statusText);
          this.setState({ submitted: false });
        });
    } else {
      NotificationManager.error("Please log in to make reservation.");
    }
  }
  renderRows(rows, seats) {

    const rowsRendered = [];
    if (this.state.allSeats.length > 0) {

      for (let i = 0; i < rows; i++) {
        const startingIndex = i * seats;
        const maxIndex = (i + 1) * seats;

        rowsRendered.push(<tr key={i}>{this.renderSeats(seats, i, startingIndex, maxIndex)}</tr>);
      }
    }
    return rowsRendered;
  }

  checkIfSeatIsTaken(currentSeatId) {
    for (let i = 0; i < this.state.reservedSeats.length; i++) {
      if (this.state.reservedSeats[i].seatId === currentSeatId) {
        return true;
      }
    }
    return false;
  }

  checkIfSeatIsCurrentlyReserved(currentSeatId) {
    return this.state.currentReservationSeats.includes(currentSeatId);
  }

  getSeatByPosition(row, number) {
    for (let i = 0; i < this.state.allSeats.length; i++) {
      if (this.state.allSeats[i].number === number && this.state.allSeats[i].row === row) {
        return this.state.allSeats[i];
      }
    }
  }

  getSeatById(seatId) {
    for (let i = 0; i < this.state.allSeats.length; i++) {
      if (this.state.allSeats[i].id === seatId) {
        return this.state.allSeats[i];
      }
    }
  }

  getAllButtons() {
    if (!this.allButtons) {
      this.allButtons = document.getElementsByClassName("seat");
      for (let i = 0; i < this.allButtons.length; i++) {
        let seat = this.getSeatById(this.allButtons[i].value);
      }
    }
  }

  markSeatAsGreenish(seatId) {
    this.getAllButtons();
    for (let i = 0; i < this.allButtons.length; i++) {
      if (seatId === this.allButtons[i].value) {
        this.allButtons[i].className = "seat nice-green-color";
      }
    }
  }

  getButtonBySeatId(seatId) {
    this.getAllButtons();
    for (let i = 0; i < this.allButtons.length; i++) {
      if (seatId === this.allButtons[i].value) {
        return this.allButtons[i];
      }
    }
  }

  markSeatAsBlue(seatId) {
    this.getAllButtons();
    for (let i = 0; i < this.allButtons.length; i++) {
      if (seatId === this.allButtons[i].value) {
        this.allButtons[i].className = "seat";
      }
    }
  }

  markWholeRowSeatsAsBlue() {
    this.getAllButtons();
    for (let i = 0; i < this.allButtons.length; i++) {
      if (this.allButtons[i].className !== "seat seat-taken") {
        this.allButtons[i].className = "seat";
      }
    }
  }

  renderSeats(seats, row, startingIndex, maxIndex) {
    const { allSeats } = this.state;
    let renderedSeats = [];
    let seatIndex = startingIndex;
    if (this.state.allSeats.length > 0) {
      for (let i = 0; i < seats; i++) {
        let currentSeatId = allSeats[seatIndex].id;
        let currentlyReserved = this.state.currentReservationSeats.filter(seat => seat === currentSeatId).length > 0;
        let seatTaken = this.state.reservedSeats.filter(seat => seat.seatId === currentSeatId).length > 0;

        renderedSeats.push(
          <button
            onClick={
              e => {
                let currentRow = row;
                let currentNumber = i;
                let currSeatId = currentSeatId;

                let leftSeatIsCurrentlyReserved;
                let leftSeatIsTaken;
                let rightSeatIsCurrentlyReserved;
                let rightSeatIsTaken;
                let leftSeatProperties = this.getSeatByPosition(currentRow + 1, currentNumber);
                let rightSeatProperties = this.getSeatByPosition(currentRow + 1, currentNumber + 2);

                if (leftSeatProperties) {
                  leftSeatIsCurrentlyReserved = this.checkIfSeatIsCurrentlyReserved(leftSeatProperties.id);
                  leftSeatIsTaken = this.checkIfSeatIsTaken(leftSeatProperties.id);
                }

                if (rightSeatProperties) {
                  rightSeatIsCurrentlyReserved = this.checkIfSeatIsCurrentlyReserved(rightSeatProperties.id);
                  rightSeatIsTaken = this.checkIfSeatIsTaken(rightSeatProperties.id);
                }

                if (!this.checkIfSeatIsCurrentlyReserved(currSeatId)) {

                  if (this.state.currentReservationSeats.length !== 0 && this.getButtonBySeatId(currentSeatId).className !== "seat nice-green-color") {
                    return;
                  }
                  if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                    this.markSeatAsGreenish(leftSeatProperties.id);
                  }
                  if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                    this.markSeatAsGreenish(rightSeatProperties.id);
                  }
                  if (this.state.currentReservationSeats.includes(currentSeatId) === false) {
                    this.state.currentReservationSeats.push(currentSeatId);
                  }
                } else {

                  if (leftSeatIsCurrentlyReserved && rightSeatIsCurrentlyReserved) {
                    this.markWholeRowSeatsAsBlue();
                    this.state.currentReservationSeats = [];
                  } else {
                    this.state.currentReservationSeats.splice(this.state.currentReservationSeats.indexOf(currentSeatId), 1);
                    if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                      this.markSeatAsBlue(leftSeatProperties.id);
                    }
                    if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                      this.markSeatAsBlue(rightSeatProperties.id);
                    }

                    if (leftSeatIsCurrentlyReserved || rightSeatIsCurrentlyReserved) {
                      setTimeout(() => { this.markSeatAsGreenish(currentSeatId); }, 150);
                    }
                  }
                  this.setState({ currentReservationSeats: this.state.currentReservationSeats });
                }
                this.setState({ currentReservationSeats: this.state.currentReservationSeats });
              }}
            class={seatTaken ? "seat seat-taken" : currentlyReserved ? "seat seat-current-reservation" : "seat"}
            value={currentSeatId}
            key={"row: " + row + ", seat: " + i}
          >
            <FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch} />
          </button>
        );
        if (seatIndex < maxIndex) {
          seatIndex += 1;
        }
      }
    }

    return renderedSeats;
  }

  getRoundedRating(rating) {
    const result = Math.round(rating);
    return <span className="float-right">Rating: {result}/10</span>
  }


  fillTableWithData() {
    let auditorium = this.renderRows(this.state.maxNumberOfRow, this.state.maxRow);
    return <Card.Body>
      <Card.Title><span className="card-title-font">{this.state.movies.title}</span>
        <span className="float-right">{this.getRoundedRating(this.state.movies.rating)}</span></Card.Title>
      <hr />
      <Card.Subtitle className="mb-2 text-muted">Year of production: {this.state.movies.year}
        <span className="float-right">Time of projection: {this.state.slicedTime}h</span></Card.Subtitle>
      <hr />
      <Card.Text>
        <Row className="mt-2">
          <Col className="justify-content-center align-content-center">
            <form>
              <div className="payment">
                <h4 className="text-center">Choose your seat(s):</h4>
                <table class="payment-table">
                  <thead class="payment-table__head">
                    <tr class="payment-table__row">
                      <th class="payment-table__cell">Ulaznice</th>
                      <th class="payment-table__cell">Cena</th>
                      <th class="payment-table__cell">Ukupno</th>
                    </tr>
                  </thead>
                  <tbody class="payment-table__row">
                    <tr>
                      <td class="payment-table__cell"><span>{this.state.currentReservationSeats.length}</span></td>
                      <td class="payment-table__cell">350,00 RSD</td>
                      <td class="payment-table__cell">{this.state.currentReservationSeats.length * 350},00 RSD</td>
                    </tr>
                  </tbody>
                </table>
                <button onClick={(e) => this.tryReservation(e)} class="btn-payment">Confirm<FontAwesomeIcon className="text-primary mr-2 fa-1x btn-payment__icon" icon={faShoppingCart} /></button>
              </div>
            </form>
            <div>
              <Row className="justify-content-center">
                <table className="table-cinema-auditorium">
                  <tbody>
                    {auditorium}
                  </tbody>
                </table>
                <div className="text-center text-white font-weight-bold cinema-screen">
                  CINEMA SCREEN
                  </div>
              </Row>
            </div>
          </Col>
        </Row>
        <hr />
      </Card.Text>
    </Card.Body>
  }

  render() {

    const showTable = this.fillTableWithData();
    return (

      <Container>
        <Row className="justify-content-center">
          <Col>
            <Card className="mt-5 card-width">
              {showTable}
            </Card>
          </Col>
        </Row>
      </Container>
    );
  }
}

export default ProjectionDetails;