import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import styles from './App.module.css';

import Navigation from './components/Navigation';
import Footer from './components/Footer';
import Sidebar from './components/Sidebar';
import { About, Contact, Home, Layout, Rss, Subscribe } from './pages';

function App() {
	return (
		<Router>
			<Navigation />
			<div className={`container-fluid ${styles.content}`}>
				<div className='row'>
					<div className={`${styles.main} col-9`}>
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
								<Route
									path='newsletter/subscribe'
									element={<Subscribe />}
								/>
							</Route>
						</Routes>
					</div>
					<div className={`${styles.sidebar} col-3 border-start`}>
						<Sidebar />
					</div>
				</div>
			</div>
			<Footer />
		</Router>
	);
}

export default App;
