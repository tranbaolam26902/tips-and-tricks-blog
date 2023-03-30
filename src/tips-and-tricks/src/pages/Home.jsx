import { useState, useEffect } from 'react';

import PostItem from '../components/PostItem';
import { getPosts } from '../services/blogRepository';

export default function Home() {
	// Component's states
	const [posts, setPosts] = useState([]);

	useEffect(() => {
		document.title = 'Trang chủ';
		fetchPosts();

		async function fetchPosts() {
			const data = await getPosts();
			if (data) setPosts(data.items);
			else setPosts([]);
		}
	}, []);

	return (
		<>
			{posts.length > 0 ? (
				<div className='p-4'>
					{posts.map((post) => (
						<PostItem key={post.id} post={post} />
					))}
				</div>
			) : (
				<h1 className='text-danger'>Không có bài viết</h1>
			)}
		</>
	);
}
