import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { getAuthorBySlug } from '../../../services/authors';

import PostsFilter from '../../../components/blog/PostsFilter';

export default function PostsByAuthor() {
	// Component's variables
	const params = useParams();

	// Component's states
	const [author, setAuthor] = useState({});

	useEffect(() => {
		fetchAuthor();

		async function fetchAuthor() {
			const data = await getAuthorBySlug(params.slug);
			if (data) setAuthor(data);
			else setAuthor({});
		}
	}, [params]);

	return (
		<div className='p-4'>
			<h1 className='mb-4'>
				Danh sách bài viết của tác giả: "{author.fullName}"
			</h1>
			<PostsFilter postQuery={{ authorSlug: params.slug }} />
		</div>
	);
}
