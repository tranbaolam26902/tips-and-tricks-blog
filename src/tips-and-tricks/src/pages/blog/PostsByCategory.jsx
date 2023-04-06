import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { getCategoryBySlug } from '../../services/categories';

import PostsFilter from '../../components/PostsFilter';

export default function PostsByCategory() {
	// Component's variables
	const params = useParams();

	const [category, setCategory] = useState({});

	useEffect(() => {
		fetchCategory();

		async function fetchCategory() {
			const data = await getCategoryBySlug(params.slug);
			if (data) setCategory(data);
			else setCategory({});
		}
	}, [params]);

	return (
		<div className='p-4'>
			<h1 className='mb-4'>
				Danh sách bài viết thuộc danh mục: "{category.name}"
			</h1>
			<PostsFilter postQuery={{ categorySlug: params.slug }} />
		</div>
	);
}
