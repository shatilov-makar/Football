import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Alert, Form, Button } from 'react-bootstrap';
import axios from 'axios';
import TeamCreateForm from './TeamCreateForm';
import moment from 'moment/moment';

export default function PlayerForm() {
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [gender, setGender] = useState(0);
  const [birthday, setBirthday] = useState(moment(new Date()).format('YYYY-MM-DD'));
  const [team, setTeam] = useState();
  const [country, setCountry] = useState();
  const [teams, setTeams] = useState([]);
  const [countries, setCountries] = useState([]);
  const [isUpdateForm, setIsUpdateForm] = useState(false);
  const [showTeamForm, setShowTeamForm] = useState(false);
  const [successAction, setSuccessAction] = useState(false);
  const [errorAction, setErrorAction] = useState('');
  const { player_id } = useParams();

  const getData = async (url, next) => {
    const data = await axios
      .get(url)
      .then((req) => req.data)
      .catch((error) => {
        setSuccessAction(false);
        setErrorAction(error.message);
      });
    next(data);
  };

  const initPlayerStates = (playerData) => {
    if (playerData) {
      setGender(playerData.gender || 0);
      setName(playerData.name || '');
      setSurname(playerData.surname || '');
      setBirthday(moment(playerData.birthday || new Date()).format('YYYY-MM-DD'));
      setTeam(playerData.teamID || '');
      setCountry(playerData.countryID || '');
    } else {
      setErrorAction('Не удалось загрузить данные пользователя');
    }
  };

  useEffect(() => {
    if (player_id) {
      setIsUpdateForm(true);
      getData(`/api/players/${player_id}`, initPlayerStates);
    } else {
      setIsUpdateForm(false);
      initPlayerStates({});
    }
    getData('/api/teams', (data) => setTeams(data));
    getData('/api/countries', (data) => setCountries(data));
  }, [player_id]);

  const sendPlayer = async (e, method) => {
    e.preventDefault();
    const req = {
      id: player_id,
      name,
      surname,
      gender,
      birthday,
      teamID: team,
      countryID: country,
    };
    setSuccessAction(false);
    await axios[method]('/api/players', req, {
      headers: { 'Content-Type': 'application/json; charset=UTF-8' },
    })
      .then(async (result) => {
        setSuccessAction(true);
        setErrorAction('');
        if (!isUpdateForm) initPlayerStates({});
      })
      .catch(async (error) => {
        console.error(error);
        setErrorAction([Object.values(error.response.data)[0]][0][0]);
        setSuccessAction(false);
      });
  };

  return (
    <div className="container p-5">
      <h2 className="text-center">
        {isUpdateForm ? `Обновите данные` : `Создайте нового футболиста`}
      </h2>
      <Form onSubmit={(e) => sendPlayer(e, isUpdateForm ? 'put' : 'post')} id="playerForm">
        <Form.Group className="mb-3" controlId="formBasicName">
          <Form.Label>Имя</Form.Label>
          <Form.Control
            required
            type="text"
            placeholder="Введите имя"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="formBasicSurname">
          <Form.Label>Фамилия</Form.Label>
          <Form.Control
            required
            type="text"
            placeholder="Введите фамилию"
            value={surname}
            onChange={(e) => setSurname(e.target.value)}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicGender">
          <Form.Label>Пол</Form.Label>
          <Form.Check
            checked={gender === 0}
            name="gender"
            id={'male'}
            type="radio"
            label={'Мужчина'}
            value={0}
            onChange={(e) => setGender(0)}
          />
          <Form.Check
            checked={gender === 1}
            name="gender"
            id={'female'}
            type="radio"
            label={'Женщина'}
            value={1}
            onChange={(e) => setGender(1)}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicBirthday">
          <Form.Label>Дата рождения</Form.Label>
          <Form.Control
            required
            type="date"
            placeholder="Введите дату рождения"
            value={birthday}
            onChange={(e) => setBirthday(e.target.value)}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicTeam">
          <Form.Label>За какую команду выступает</Form.Label>
          <Form.Select
            required
            aria-label="За какую команду выступает"
            value={team}
            onChange={(e) => setTeam(e.target.value)}
            defaultValue={team ? team : ''}>
            <option value="" disabled>
              Выберите команду
            </option>
            {teams.map((team) => (
              <option value={team.id} key={team.id}>
                {team.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicCreateNewTeam">
          <Button
            variant="secondary"
            onClick={() => {
              setShowTeamForm(true);
            }}>
            Создать новую команду
          </Button>
          <TeamCreateForm
            show={showTeamForm}
            onHide={() => {
              setShowTeamForm(false);
              getData('/api/teams', (data) => setTeams(data));
            }}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicNationality">
          <Form.Label>Национальность</Form.Label>
          <Form.Select
            required
            aria-label="Национальность"
            value={country}
            onChange={(e) => setCountry(e.target.value)}
            defaultValue={country ? country : ''}>
            <option value="" disabled>
              Выберите страну
            </option>
            {countries.map((country) => (
              <option value={country.id} key={country.id}>
                {country.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>
        {successAction && !errorAction && (
          <Alert variant="success">
            {isUpdateForm ? 'Данные пользователя обновлены' : 'Пользователь создан!'}
          </Alert>
        )}
        {!successAction && errorAction && <Alert variant="danger ">{errorAction}</Alert>}
        <Button variant="primary" type="submit" form="playerForm">
          {isUpdateForm ? 'Обновить данные' : 'Создать'}
        </Button>
      </Form>
    </div>
  );
}
