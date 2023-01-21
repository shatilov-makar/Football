import React, { useState, useEffect } from 'react';
import { Table, Alert, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import moment from 'moment/moment';
import axios from 'axios';

export default function Players() {
  const [players, setPlayers] = useState([]);

  const getPlayers = async () => {
    axios
      .get('/players')
      .then((players) => {
        setPlayers(players.data);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  useEffect(() => {
    getPlayers();
  }, []);

  const Row = ({ player }) => {
    return (
      <tr key={player.id} className="align-middle">
        <td scope="col">{player.name}</td>
        <td scope="col">{player.surname}</td>
        <td scope="col">{moment(player.birthday).format('DD.MM.YYYY')}</td>
        <td scope="col">{player.gender ? 'Ж' : 'М'}</td>
        <td scope="col">{player.teamName}</td>
        <td scope="col">{player.countryName}</td>
        <td>
          <Link to={`/player/${player.id}`}>
            <Button variant="secondary" size="sm">
              Обновить
            </Button>
          </Link>
        </td>
      </tr>
    );
  };

  const FootballPlayersTable = () => {
    return (
      <Table striped bordered hover className="mt-3">
        <thead className="align-middle">
          <tr>
            <th scope="col">Имя</th>
            <th scope="col">Фамилия</th>
            <th scope="col">Дата рождения</th>
            <th scope="col">Пол</th>
            <th scope="col">Команда</th>
            <th scope="col">Страна</th>
            <th scope="col">Обновить</th>
          </tr>
        </thead>
        <tbody>
          {players.map((player) => (
            <Row player={player} id={player.id} key={player.id} />
          ))}
        </tbody>
      </Table>
    );
  };

  return (
    <div className="container p-5">
      <div className="row min-vh-100 ">
        <div className="col d-flex flex-column align-items-center ">
          <h2 className="text-center">Список игроков</h2>
          <FootballPlayersTable />
        </div>
      </div>
    </div>
  );
}
