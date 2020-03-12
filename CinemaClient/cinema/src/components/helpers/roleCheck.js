import React, { Component } from 'react';
import { Row, Col } from 'react-bootstrap';
import { Switch, NavLink, Route } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faList, faFilm, faVideo, faTicketAlt, faBinoculars, faStar, faPlayCircle } from '@fortawesome/free-solid-svg-icons';
import {isAdmin, isSuperUser, isUser} from './../helpers/authCheck';

export const checkRole = () => {

    if(isAdmin()) 
    {
        console.log("samo admin");
        return (
            <Col lg={2} className="dashboard-navigation">
        <Row className="justify-content-center mt-2">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faFilm}/>Movie</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/AllMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/TopTenMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faStar}/>Top 10 Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2 adminSuperUser">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewMovie'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Movie</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white admin"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faBinoculars}/>Auditorium</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/AllAuditoriums'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Auditoriums</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewAuditorium'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Auditorium</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white admin"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faTicketAlt}/>Cinema</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/AllCinemas'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Cinemas</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewCinema'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Cinema</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faVideo}/>Projection</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/AllProjections'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Projections</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewProjection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Projection</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/Projection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlayCircle}/>Projections</NavLink>
        </Row>
        </Col>
        );
    }
    if(isSuperUser()===true) 
    {
        console.log("admin i superuser");
        return (
            <Col lg={2} className="dashboard-navigation">
        <Row className="justify-content-center mt-2">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faFilm}/>Movie</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/AllMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/TopTenMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faStar}/>Top 10 Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2 adminSuperUser">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewMovie'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Movie</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white admin"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faBinoculars}/>Auditorium</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/AllAuditoriums'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Auditoriums</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faVideo}/>Projection</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/AllProjections'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Projections</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink class="admin" activeClassName="active-link" to='/dashboard/NewProjection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlus}/>Add Projection</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/Projection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlayCircle}/>Projections</NavLink>
        </Row>
        </Col>
        );
    } 
    if(isUser()===true)
    {
        console.log("user");
        return (
            <Col lg={2} className="dashboard-navigation">
        <Row className="justify-content-center mt-2">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faFilm}/>Movie</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/AllMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/TopTenMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faStar}/>Top 10 Movies</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faVideo}/>Projection</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/Projection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlayCircle}/>Projections</NavLink>
        </Row>
        </Col>
        );
    }
    else {
        return (
            <Col lg={2} className="dashboard-navigation">
        <Row className="justify-content-center mt-2">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faFilm}/>Movie</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/AllMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faList}/>All Movies</NavLink>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/TopTenMovies'><FontAwesomeIcon className='text-primary mr-1' icon={faStar}/>Top 10 Movies</NavLink>
        </Row>
        <Row className="justify-content-center">
            <span className="fa-2x text-white"><FontAwesomeIcon className="text-white mr-2 fa-1x" icon={faVideo}/>Projection</span>
        </Row>
        <Row className="justify-content-center mt-2">
            <NavLink activeClassName="active-link" to='/dashboard/Projection'><FontAwesomeIcon className='text-primary mr-1' icon={faPlayCircle}/>Projections</NavLink>
        </Row>
        </Col>
        );
    }
}