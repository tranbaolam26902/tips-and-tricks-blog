import { useParams } from 'react-router-dom';

import PostsFilter from '../../../components/blog/PostsFilter';

export default function PostsByTime() {
	// Component's variables
	const params = useParams();

	return (
		<div className='p-4'>
			<h1 className='mb-4'>
				Danh sách bài viết đăng vào tháng {params.month} {params.year}
			</h1>
			<PostsFilter
				postQuery={{ year: params.year, month: params.month }}
			/>
		</div>
	);
}
