import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavbarComponent from './components/NavbarComponent';
import Players from './components/Players';
import PlayerForm from './components/PlayerForm';
import NotFound from './components/NotFound';

export default function App() {
  return (
    <div>
      <Router>
        <NavbarComponent></NavbarComponent>
        <Routes>
          <Route exec path="/" element={<Players />}></Route>
          <Route exec path="/create-player" element={<PlayerForm />}></Route>
          <Route path="/player/:player_id" element={<PlayerForm />} />
          <Route path="*" element={<NotFound></NotFound>} />
        </Routes>
      </Router>
    </div>
  );
}
