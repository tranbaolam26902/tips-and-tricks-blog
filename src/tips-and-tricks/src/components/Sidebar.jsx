import { Link } from 'react-router-dom';
import SearchForm from './SearchForm';
import {
	ArchivesWidget,
	CategoriesWidget,
	FeaturedPostsWidget,
	RandomPostsWidget,
	TagCloudWidget,
	TopAuthorsWidget,
} from './widgets';

export default function Sidebar() {
	return (
		<div className='mb-4 pt-4 ps-2'>
			<SearchForm />
			<CategoriesWidget />
			<FeaturedPostsWidget />
			<RandomPostsWidget />
			<TagCloudWidget />
			<TopAuthorsWidget />
			<ArchivesWidget />
			<Link
				to='/newsletter/subscribe'
				className='btn btn-primary py-2 w-100'
			>
				Đăng ký nhận tin mới
			</Link>
		</div>
	);
}
