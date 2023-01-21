import { Navbar, Nav, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';

export default function NavbarComponent() {
  return (
    <Navbar bg="dark" variant={'dark'} expand="lg">
      <Container>
        <Navbar.Brand as={Link} to={'/players'}>
          Каталог футболистов 3.0
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link as={Link} to={'/players'}>
              Список игроков
            </Nav.Link>
            <Nav.Link as={Link} to={'/create-player'}>
              Новый футболист
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
