import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import {
	BlogLayout,
	BlogHome,
	About,
	Contact,
	Rss,
	Subscribe,
	NotFound,
	Post,
	PostsByCategory,
	PostsByAuthor,
	PostsByTag,
	PostsByTime,
	AdminLayout,
	AdminHome,
	Categories,
	Authors,
	Tags,
	Posts,
	Comments,
} from './pages';
import Footer from './components/blog/Footer';

function App() {
	return (
		<Router>
			<Routes>
				<Route path='/' element={<BlogLayout />}>
					<Route path='/' element={<BlogHome />} />
					<Route path='blog' element={<BlogHome />} />
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
				<Route path='/admin' element={<AdminLayout />}>
					<Route path='/admin' element={<AdminHome />} />
					<Route path='/admin/categories' element={<Categories />} />
					<Route path='/admin/authors' element={<Authors />} />
					<Route path='/admin/tags' element={<Tags />} />
					<Route path='/admin/posts' element={<Posts />} />
					<Route path='/admin/comments' element={<Comments />} />
				</Route>
			</Routes>
			<Footer />
		</Router>
	);
}

export default App;
