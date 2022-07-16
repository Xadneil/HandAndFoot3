import React, { FunctionComponent, PropsWithChildren } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout: FunctionComponent<PropsWithChildren> = ({ children }) => {
  return (
    <>
      <NavMenu />
      <Container>
        {children}
      </Container>
    </>
  );
};

export default Layout;