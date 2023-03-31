import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

import { getFeaturedPosts } from '../../services/widgets';

export default function FeaturedPosts() {
	// Component's states
	const [posts, setPosts] = useState([]);

	useEffect(() => {
		fetchPosts();

		async function fetchPosts() {
			const data = await getFeaturedPosts(3);
			if (data) setPosts(data);
			else setPosts([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Bài viết nổi bật</h3>
			{posts.length > 0 && (
				<ListGroup>
					{posts.map((post, index) => {
						const postedDate = new Date(post.postedDate);
						return (
							<ListGroup.Item key={index}>
								<Link
									to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDate()}/${
										post.urlSlug
									}`}
								>
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
