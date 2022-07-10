import React, { FunctionComponent, useContext, useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { UserContext } from 'utils/context';

const NavMenu: FunctionComponent = () => {
  const [collapsed, setCollapsed] = useState(true);
  const { playerId } = useContext(UserContext);

  const toggleNavbar = () => {
    setCollapsed(!collapsed)
  };
  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">Hand And Foot</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              {(playerId !== null && playerId !== undefined) ?
                <>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/sessions">Sessions</NavLink>
                  </NavItem>
                </> : null}
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};

export default NavMenu;