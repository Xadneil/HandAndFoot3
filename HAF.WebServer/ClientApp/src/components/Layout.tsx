import React, { FunctionComponent, PropsWithChildren } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout: FunctionComponent<PropsWithChildren> = ({ children }) => {
  return (
    <>
      <NavMenu />
      <Container style={{ flex: 1 }}>
        {children}
      </Container>
    </>
  );
};

export default Layout;