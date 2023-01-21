import React, { useState, useRef, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Alert, Form, Modal, Button } from 'react-bootstrap';
import axios from 'axios';

export default function TeamCreateForm(props) {
  const [teamName, setTeamName] = useState('');
  const [errorAction, setErrorAction] = useState('');

  const postTeam = async (e) => {
    e.preventDefault();
    await axios
      .post(
        '/teams',
        { Name: teamName },
        {
          headers: { 'Content-Type': 'application/json; charset=UTF-8' },
        }
      )
      .then(async (result) => {
        props.onHide();
      })
      .catch(async (error) => {
        setErrorAction([Object.values(error.response.data)[0]][0]);
      });
  };

  return (
    <>
      <Modal {...props} centered>
        <Modal.Header closeButton>
          <Modal.Title>Создайте команду</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={postTeam}>
            <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
              <Form.Label>Введите название команды</Form.Label>
              <Form.Control
                required
                type="text"
                autoFocus
                onChange={(e) => setTeamName(e.target.value)}
              />
            </Form.Group>
            {errorAction && <Alert variant="danger ">{errorAction}</Alert>}
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={props.onHide}>
            Закрыть
          </Button>
          <Button variant="primary" onClick={postTeam}>
            Создать команду
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}
