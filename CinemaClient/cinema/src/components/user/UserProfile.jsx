import React, { Component } from 'react';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../../appSettings';
import { Row, Table } from 'react-bootstrap';
import { getUserName, getRole } from '../helpers/authCheck';
import { withRouter } from 'react-router';


class UserProfile extends Component {

    constructor(props) {
      super(props);
      this.state = {
        user: [],
        reservations: [],
        projection: []
      };
    }

    componentDidMount() {
    this.getUserByUsername();
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
              this.setState({ user: data});
              
              this.getReservationsByUserId(this.state.user.id);
            }
          })
          .catch(response => {
            NotificationManager.error(response.message || response.statusText);
            this.setState({ submitted: false });
          });
    
      }

      getReservationsByUserId(userId) {
        
        const requestOptions = {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('jwt')
          }
        };
    
        fetch(`${serviceConfig.baseURL}/api/reservations/byuserid/` + userId, requestOptions)
          .then(response => {
            if (!response.ok) {
              return;
            }
            return response.json();
          })
          .then(data => {
            if (data) {
              this.setState({ reservations: data});
              let projections = [];
              this.state.reservations.map(reservation => {
                projections.push(this.getProjectionById(reservation.projectionId));
                });
            }
          })
          .catch(response => {
            NotificationManager.error(response.message || response.statusText);
            this.setState({ submitted: false });
          });
      }

      getProjectionById(projectionId) {

        const requestOptions = {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': 'Bearer ' + localStorage.getItem('jwt')
            }
          };
      
          fetch(`${serviceConfig.baseURL}/api/projections/byprojectionid/` + projectionId, requestOptions)
            .then(response => {
              if (!response.ok) {
                return;
              }
              return response.json();
            })
            .then(data => {
              if (data) {
                this.setState({ projection: data});
              }
            })
            .catch(response => {
              NotificationManager.error(response.message || response.statusText);
              this.setState({ submitted: false });
            });
      }

    render() {
                 
        return (
            <React.Fragment>
                <Row className="no-gutters pt-2">
                    <h1 className="form-header form-heading">Hello, {this.state.user.firstName}!</h1>
                </Row>
                <Row className="no-gutters pr-5 pl-5">
                <div class="card mb-3 user-info-container">
                <div class="row no-gutters">
                    <div class="col-md-4">
                    <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcS8tVjlY8BQfSZg9SoudTWMCR6eHXpi-QHhQDUYSyjFmHYOTyyp" class="avatar-img" alt="..."/>
                    </div>
                    <div class="col-md-8">
                    <div class="card-body">
                        <h5 class="card-title">User details:</h5>
                        <p class="card-text"><strong>Full name:</strong> {this.state.user.firstName + ' ' + this.state.user.lastName}</p>
                        <p class="card-text"><strong>Bonus points: </strong> {this.state.user.bonusPoints}</p>
                        <p class="card-text"><strong>Status: </strong> {getRole()}</p>
                    </div>
                    </div>
                </div>
                </div>
                </Row>
            </React.Fragment>
        );
      }
}

export default withRouter(UserProfile);