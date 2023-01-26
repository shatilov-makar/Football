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
  const [isUpdatedForm, setIsUpdatedForm] = useState(false);
  const [showTeamForm, setShowTeamForm] = useState(false);
  const [successAction, setSuccessAction] = useState(false);
  const [errorAction, setErrorAction] = useState('');
  const { player_id } = useParams();

  const getData = async (url, f) => {
    const data = await axios
      .get(url)
      .then((req) => req.data)
      .catch((error) => {
        setSuccessAction(false);
        setErrorAction(error.message);
      });
    f(data);
  };

  const initPlayer = async (playerID) => {
    getData(`/api/players/${playerID}`, (playerData) => {
      if (playerData) {
        setGender(playerData.gender);
        setName(playerData.name);
        setSurname(playerData.surname);
        setBirthday(moment(playerData.birthday).format('YYYY-MM-DD'));
        setTeam(playerData.teamID);
        setCountry(playerData.countryID);
      } else {
        setErrorAction('Не удалось загрузить данные пользователя');
      }
    });
  };

  const setEmptyPlayer = async () => {
    setGender(0);
    setName('');
    setSurname('');
    setBirthday(new Date());
    setTeam('');
    setCountry('');
  };

  const checkFileds = () => {
    return name && surname && birthday && team && country;
  };

  useEffect(() => {
    if (player_id) {
      setIsUpdatedForm(true);
      initPlayer(player_id);
    }
    getData('/api/teams', (data) => setTeams(data));
    getData('/api/countries', (data) => setCountries(data));
  }, [player_id]);

  const createPlayer = async (e) => {
    e.preventDefault();

    const req = {
      name,
      surname,
      gender,
      birthday,
      teamID: team,
      countryID: country,
    };
    if (!checkFileds()) {
      return;
    }
    setSuccessAction(false);
    await axios
      .post('/api/players', req, {
        headers: { 'Content-Type': 'application/json; charset=UTF-8' },
      })
      .then(async (result) => {
        setSuccessAction(true);
        setEmptyPlayer();
        setErrorAction('');
      })
      .catch(async (error) => {
        console.log(error);
        setErrorAction([Object.values(error.response.data)[0]][0]);
        setSuccessAction(false);
      });
  };

  const updatePlayer = async (e) => {
    e.preventDefault();
    if (!checkFileds()) {
      return;
    }
    setSuccessAction(false);
    const req = {
      id: player_id,
      name,
      surname,
      birthday,
      gender,
      teamID: team,
      countryID: country,
    };
    await axios
      .put(`/api/players`, req, {
        headers: { 'Content-Type': 'application/json; charset=UTF-8' },
      })
      .then(async (result) => {
        setSuccessAction(true);
        setErrorAction('');
      })
      .catch(async (error) => {
        setErrorAction([Object.values(error.response.data)[0]][0]);
        setSuccessAction(false);
      });
  };

  return (
    <div className="container p-5">
      <h2 className="text-center">
        {isUpdatedForm ? `Обновите данные` : `Создайте нового футболиста`}
      </h2>
      <Form onSubmit={(e) => (isUpdatedForm ? updatePlayer(e) : createPlayer(e))} id="playerForm">
        <Form.Group className="mb-3" controlId="formBasicName">
          <Form.Label>Имя</Form.Label>
          <Form.Control
            required
            type="text"
            placeholder="Введите имя"
            value={name}
            onChange={(e) => setName(e.target.value === ' ' ? '' : e.target.value)}
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="formBasicSurname">
          <Form.Label>Фамилия</Form.Label>
          <Form.Control
            required
            type="text"
            placeholder="Введите фамилию"
            value={surname}
            onChange={(e) => setSurname(e.target.value === ' ' ? '' : e.target.value)}
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
        <>
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
        </>
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
            {isUpdatedForm ? 'Данные пользователя обновлены' : 'Пользователь создан!'}
          </Alert>
        )}
        {!successAction && errorAction && <Alert variant="danger ">{errorAction}</Alert>}
        <Button variant="primary" type="submit" form="playerForm">
          {isUpdatedForm ? 'Обновить данные' : 'Создать'}
        </Button>
      </Form>
    </div>
  );
}
