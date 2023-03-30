import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import './App.css';

import Navigation from './components/Navigation';

function App() {
	return (
		<Router>
			<Navigation />
		</Router>
	);
}

export default App;
