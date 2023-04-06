import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

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
import Footer from './components/Footer';

function App() {
	return (
		<Router>
			<Routes>
				<Route path='/' element={<Layout />}>
					<Route path='/' element={<Home />} />
					<Route path='blog' element={<Home />} />
					<Route path='blog/contact' element={<Contact />} />
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
					<Route path='blog/tag/:slug' element={<PostsByTag />} />
					<Route
						path='blog/archive/:year/:month'
						element={<PostsByTime />}
					/>
					<Route path='blog/post/:slug' element={<Post />} />
					<Route path='*' element={<NotFound />} />
				</Route>
			</Routes>
			<Footer />
		</Router>
	);
}

export default App;
