import { useState, useEffect } from 'react';

import PostItem from '../components/PostItem';

export default function Home() {
	// Component's states
	const [posts, setPosts] = useState([]);

	useEffect(() => {
		document.title = 'Trang chủ';
	}, []);

	return (
		<>
			{posts.length > 0 && (
				<div className='p-4'>
					{posts.map((post) => (
						<PostItem key={post.id} post={post} />
					))}
				</div>
			)}
		</>
	);
}
