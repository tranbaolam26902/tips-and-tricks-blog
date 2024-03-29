import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import {
	BlogLayout,
	BlogHome,
	About,
	Contact,
	Rss,
	Subscribe,
	Post,
	PostsByCategory,
	PostsByAuthor,
	PostsByTag,
	PostsByTime,
	AdminLayout,
	AdminHome,
	Categories,
	EditCategory,
	Authors,
	EditAuthor,
	Tags,
	EditTag,
	Posts,
	EditPost,
	Comments,
	NotFound,
	BadRequest,
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
				</Route>
				<Route path='/admin' element={<AdminLayout />}>
					<Route path='/admin' element={<AdminHome />} />
					<Route path='/admin/categories' element={<Categories />} />
					<Route
						path='/admin/categories/edit'
						element={<EditCategory />}
					/>
					<Route
						path='/admin/categories/edit/:id'
						element={<EditCategory />}
					/>
					<Route path='/admin/authors' element={<Authors />} />
					<Route
						path='/admin/authors/edit'
						element={<EditAuthor />}
					/>
					<Route
						path='/admin/authors/edit/:id'
						element={<EditAuthor />}
					/>
					<Route path='/admin/tags' element={<Tags />} />
					<Route path='/admin/tags/edit' element={<EditTag />} />
					<Route path='/admin/tags/edit/:id' element={<EditTag />} />
					<Route path='/admin/posts' element={<Posts />} />
					<Route path='/admin/posts/edit' element={<EditPost />} />
					<Route
						path='/admin/posts/edit/:id'
						element={<EditPost />}
					/>
					<Route path='/admin/comments' element={<Comments />} />
				</Route>
				<Route path='*' element={<NotFound />} />
				<Route path='/400' element={<BadRequest />} />
			</Routes>
			<Footer />
		</Router>
	);
}

export default App;
