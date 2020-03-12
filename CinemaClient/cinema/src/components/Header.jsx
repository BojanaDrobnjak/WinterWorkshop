import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Navbar, Nav, Form, FormControl, Button } from 'react-bootstrap';
import { NotificationManager } from 'react-notifications';
import { serviceConfig } from '../appSettings';
import { getTokenExp, isGuest } from '../../src/components/helpers/authCheck';

class Header extends Component {
  constructor(props) {
    super(props);
    this.state = {
      username: '',
      submitted: false,
      token: false,
      shouldHide: true
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getToken = this.getToken.bind(this);
    this.handleSubmitLogout = this.handleSubmitLogout.bind(this);
    setTimeout(() => {
      if (localStorage.getItem("userLoggedIn") !== null) {
        this.hideLoginButtonElement();
      } else {
        this.hideLogoutButtonElement();
      }
    }, 500
    );
  }

  componentDidMount() {
    let tokenExp = getTokenExp() ;
    let currentTimestamp = +new Date();
    
    if ((tokenExp * 1000)> currentTimestamp) {
      console.log("token jos nije istekao ");
    }
    if (!tokenExp || ((tokenExp * 1000) < currentTimestamp)) {
      this.getTokenForGuest();
    }
  }

  handleChange(e) {
    const { id, value } = e.target;
    this.setState({ [id]: value });
  }

  handleSubmit(e) {
    e.preventDefault();

    this.setState({ submitted: true });
    const { username } = this.state;
    if (username) {

      this.login();

    } else {
      this.setState({ submitted: false });
    }

  }

  handleSubmitLogout(e) {
    e.preventDefault();
    localStorage.removeItem("userLoggedIn");
    this.setState({ submitted: true });
    //obrisati ovo
    this.token = false;
    this.getTokenForGuest();
  }

  //OVO BI TREBALO DA SE ZAVRSI MATIJA!!!
  hideLoginButtonElement() {

    let loginButton = document.getElementById('login');
    if (loginButton) {
      loginButton.style.display = "none";
    }
    let logoutButton = document.getElementById('logout');
    if (logoutButton) {
      logoutButton.style.display = "block";
    }
    document.getElementById("username").style.display = "none";
  }

  hideLogoutButtonElement() {
    let loginButton = document.getElementById('login');

    if (loginButton) {
      loginButton.style.display = "block";
    }
    let logoutButton = document.getElementById('logout');
    if (logoutButton) {
      logoutButton.style.display = "none";
    }
    document.getElementById("username").style.display = "block";
  }


  login() {
    localStorage.setItem('userLoggedIn', true);
    const { username } = this.state;

    const requestOptions = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('jwt')
      }
    };

    fetch(`${serviceConfig.baseURL}/api/users/byusername/${username}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        this.state.token = true;
        var isGuest = false;
        if (data.userName) {
          this.setState({ shouldHide: false });
          if (!data.isAdmin && !data.isSuperUser && !data.isUser) {
            console.log(data.isAdmin);
            isGuest = true;
          }
          this.getToken(data.isAdmin, data.isSuperUser, data.isUser, isGuest);
          NotificationManager.success('Welcome, ' + data.firstName + '!');
          console.log(data.isUser + " :user");
          console.log(data.isAdmin + " :admin");
          console.log(data.isSuperUser + " :superUser");
          console.log(isGuest + " :guest"); 
         
        }

      })
      .catch(response => {
        NotificationManager.error('Username does not exists.');
        this.setState({ submitted: false });
      });
  }

  getToken(IsAdmin, isSuperUser, isUser, isGuest) {
    const { username } = this.state;

    const requestOptions = {
      method: 'GET'
    };
    fetch(`${serviceConfig.baseURL}/get-token?name=${username}&admin=${IsAdmin}&superUser=${isSuperUser}&user=${isUser}&guest=${isGuest}`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        if (data.token) {
          localStorage.setItem("jwt", data.token);
          setTimeout(() => {
            window.location.reload();
           }, 500);
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
  }

  getTokenForGuest() {

    const requestOptions = {
      method: 'GET'

    };
    fetch(`${serviceConfig.baseURL}/get-token?guest=true`, requestOptions)
      .then(response => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then(data => {
        this.setState({ shouldHide: true });
        if (data.token) {
          localStorage.setItem("jwt", data.token);
          window.location.reload();
        }
      })
      .catch(response => {
        NotificationManager.error(response.message || response.statusText);
        this.setState({ submitted: false });
      });
    this.state.token = true;

  }

  refreshPage() {
    window.location.reload(true);
  }

  render() {
    const { username } = this.state;

    return (
      <Navbar bg="dark" expand="lg">
        <Navbar.Brand className="text-info font-weight-bold text-capitalize"><Link className="text-decoration-none" to='/dashboard/Projection'>Cinema 9</Link></Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" className="text-white" />
        <Navbar.Collapse id="basic-navbar-nav" className="text-white">
          <Nav className="mr-auto text-white" >
          </Nav>
          <Form inline onSubmit={(e) => this.handleSubmit(e)}>
            <FormControl type="text" placeholder="Username"
              id="username"
              value={username}
              onChange={this.handleChange}
              className="mr-sm-2" />
            <Button type="submit" variant="outline-success" id="login">Login</Button>
          </Form>
          <Form inline onSubmit={(e) => this.handleSubmitLogout(e)}>
            <Button type="submit" variant="outline-danger" id="logout">Logout</Button>
          </Form>
        </Navbar.Collapse>
      </Navbar>
    );

  }
}

export default Header;