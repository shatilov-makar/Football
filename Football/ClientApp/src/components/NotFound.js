import { Link } from 'react-router-dom';

export default function NotFound() {
  return (
    <div className="d-flex align-items-center justify-content-center" style={{ height: '90vh' }}>
      <div className="text-center">
        <h1 className="display-1 fw-semibold">404</h1>
        <p className="fs-3 fw-semibold"> Page not found</p>
        <p className="lead">Запрашиваемая страница не найдена</p>
        <Link to="/" className="btn btn-primary">
          Перейти к списку игроков
        </Link>
      </div>
    </div>
  );
}
