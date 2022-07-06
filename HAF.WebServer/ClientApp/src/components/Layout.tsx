import React, { FunctionComponent, PropsWithChildren } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout: FunctionComponent<PropsWithChildren> = ({ children }) => {
  return (
    <div>
      <NavMenu />
      <Container>
        {children}
      </Container>
    </div>
  );
};

export default Layout;