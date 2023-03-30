import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import './App.css';

import Navigation from './components/Navigation';
import Sidebar from './components/Sidebar';

function App() {
	return (
		<Router>
			<Navigation />
			<div className='container-fluid'>
				<div className='row'>
					<div className='col-9'></div>
					<div className='col-3 border-start'>
						<Sidebar />
					</div>
				</div>
			</div>
		</Router>
	);
}

export default App;
