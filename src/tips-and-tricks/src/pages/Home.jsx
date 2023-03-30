import { useState, useEffect, useMemo } from 'react';
import { useLocation } from 'react-router-dom';

import { getPosts } from '../services/blogRepository';

import PostItem from '../components/PostItem';
import Pager from '../components/Pager';

export default function Home() {
	// Component's states
	const [posts, setPosts] = useState([]);
	const [metadata, setMetadata] = useState([]);

	// Component's functions
	const useQuery = () => {
		const { search } = useLocation();
		return useMemo(() => new URLSearchParams(search), [search]);
	};

	// Component's variables
	const query = useQuery();
	const keyword = query.get('Keyword') ?? '';
	const pageSize = query.get('PageSize') ?? 10;
	const pageNumber = query.get('PageNumber') ?? 1;

	useEffect(() => {
		document.title = 'Trang chủ';
		fetchPosts();

		async function fetchPosts() {
			const data = await getPosts(keyword, pageSize, pageNumber);
			if (data) {
				setPosts(data.items);
				setMetadata(data.metadata);
			} else setPosts([]);
		}
	}, [keyword, pageSize, pageNumber]);

	useEffect(() => {
		window.scrollY = 0;
	}, [posts]);

	return (
		<>
			{posts.length > 0 ? (
				<div className='p-4'>
					{posts.map((post) => (
						<PostItem key={post.id} post={post} />
					))}
					<Pager postQuery={{ keyword }} metadata={metadata} />
				</div>
			) : (
				<h1 className='text-danger'>Không có bài viết</h1>
			)}
		</>
	);
}
