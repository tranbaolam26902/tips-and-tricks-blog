import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { getTagBySlug } from '../../../services/tags';

import PostsFilter from '../../../components/blog/PostsFilter';

export default function PostsByTag() {
	// Component's variables
	const params = useParams();

	const [tag, setTag] = useState({});

	useEffect(() => {
		fetchTag();

		async function fetchTag() {
			const data = await getTagBySlug(params.slug);
			if (data) setTag(data);
			else setTag({});
		}
	}, [params]);

	return (
		<div className='p-4'>
			<h1 className='mb-4'>
				Danh sách bài viết có chứa thẻ: "{tag.name}"
			</h1>
			<PostsFilter postQuery={{ tagSlug: params.slug }} />
		</div>
	);
}
