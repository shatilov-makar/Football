import React, { useState, useEffect, useRef } from 'react';
import { Table, Alert, Button, Pagination } from 'react-bootstrap';
import { Link, useSearchParams } from 'react-router-dom';
import moment from 'moment/moment';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';

export default function Players() {
  const [players, setPlayers] = useState([]);
  const [pages, setPages] = useState([]);
  const [searchParams, setSearchParams] = useSearchParams();

  const PageNumber = Number(searchParams.get('PageNumber') || 1);
  const PageSize = Number(searchParams.get('PageSize') || 15);

  const playersOnScreen = useRef(null);

  playersOnScreen.current = players;

  const getPlayers = () => {
    console.log(PageNumber, PageSize);
    axios
      .get('api/players', { params: { PageNumber, PageSize } })
      .then((players) => {
        setPlayers(players.data);
        setPages(getPages(players));
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const setupSignalRConnection = () => {
    const hubConnection = new signalR.HubConnectionBuilder().withUrl('api/playersHub').build();
    hubConnection.start().then((result) => {
      hubConnection.on('SendPlayerToUsers', updatePlayersList);
    });
    return hubConnection;
  };

  useEffect(() => {
    getPlayers();
    const connection = setupSignalRConnection();
    return () => connection.stop();
  }, []);

  const updatePlayersList = (newPlayer) => {
    const updatedPlayers = [...playersOnScreen.current];
    const playerIdAmongPlayers = playersOnScreen.current.find((p) => p.id === newPlayer.id);
    if (playerIdAmongPlayers) {
      setPlayers(updatedPlayers.map((p) => (p.id === newPlayer.id ? newPlayer : p)));
    } else if (playersOnScreen.current.length < PageSize) {
      updatedPlayers.push(newPlayer);
      setPlayers(updatedPlayers);
    }
  };

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
          <Pagination>{pages}</Pagination>
        </div>
      </div>
    </div>
  );
}

const getPages = (players) => {
  let items = [];
  const metadata = JSON.parse(players.headers['x-pagination']);
  items.push(
    <Pagination.Prev
      key={0}
      disabled={metadata.CurrentPage === 1}
      href={`/?PageNumber=${metadata.CurrentPage - 1}&PageSize=${15}`}
    />
  );
  for (let number = 1; number <= metadata.TotalPages; number++) {
    items.push(
      <Pagination.Item
        key={number}
        active={number === metadata.CurrentPage}
        href={`/?PageNumber=${number}&PageSize=${15}`}>
        {number}
      </Pagination.Item>
    );
  }
  items.push(
    <Pagination.Next
      key={metadata.TotalPages + 1}
      disabled={metadata.CurrentPage === metadata.TotalPages}
      href={`/?PageNumber=${metadata.CurrentPage + 1}&PageSize=${15}`}
    />
  );
  return items;
};
