import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import styles from './App.module.css';

import Navigation from './components/Navigation';
import Footer from './components/Footer';
import Sidebar from './components/Sidebar';
import {
	About,
	Contact,
	Home,
	Layout,
	Rss,
	Subscribe,
	NotFound,
	PostsByCategory,
	PostsByAuthor,
	PostsByTag,
	PostsByTime,
	Post,
} from './pages';

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
								<Route
									path='blog/category/:slug'
									element={<PostsByCategory />}
								/>
								<Route
									path='blog/author/:slug'
									element={<PostsByAuthor />}
								/>
								<Route
									path='blog/tag/:slug'
									element={<PostsByTag />}
								/>
								<Route
									path='blog/archive/:year/:month'
									element={<PostsByTime />}
								/>
								<Route
									path='blog/post/:slug'
									element={<Post />}
								/>
								<Route path='*' element={<NotFound />} />
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
