import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import './App.css';
import Footer from './components/Footer';

import Navigation from './components/Navigation';
import Sidebar from './components/Sidebar';
import { About, Contact, Home, Layout, Rss } from './pages';

function App() {
	return (
		<Router>
			<Navigation />
			<div className='container-fluid'>
				<div className='row'>
					<div className='col-9'>
						<Routes>
							<Route path='/' element={<Layout />}>
								<Route path='/' element={<Home />} />
								<Route path='blog' element={<Home />} />
								<Route
									path='blog/contact'
									element={<Contact />}
								/>
								<Route path='blog/about' element={<About />} />
								<Route path='blog/rss' element={<Rss />} />
							</Route>
						</Routes>
					</div>
					<div className='col-3 border-start'>
						<Sidebar />
					</div>
				</div>
			</div>
			<Footer />
		</Router>
	);
}

export default App;
