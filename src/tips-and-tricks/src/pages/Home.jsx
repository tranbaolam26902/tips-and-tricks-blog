import { useEffect } from 'react';

import PostItem from '../components/PostItem';

export default function Home({ posts }) {
	useEffect(() => {
		document.title = 'Trang chủ';
	}, []);

	return (
		<>
			{posts.length && (
				<div className='p-4'>
					{posts.map((post) => (
						<PostItem key={post.id} post={post} />
					))}
				</div>
			)}
		</>
	);
}
