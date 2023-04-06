import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

import { getRandomPosts } from '../../../services/widgets';

export default function RandomPostsWidget() {
	// Component's states
	const [posts, setPosts] = useState([]);

	useEffect(() => {
		fetchPosts();

		async function fetchPosts() {
			const data = await getRandomPosts(5);
			if (data) setPosts(data);
			else setPosts([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Bài viết ngẫu nhiên</h3>
			{posts.length > 0 && (
				<ListGroup>
					{posts.map((post, index) => {
						return (
							<ListGroup.Item key={index}>
								<Link to={`/blog/post/${post.urlSlug}`}>
									{post.title}
								</Link>
							</ListGroup.Item>
						);
					})}
				</ListGroup>
			)}
		</div>
	);
}
